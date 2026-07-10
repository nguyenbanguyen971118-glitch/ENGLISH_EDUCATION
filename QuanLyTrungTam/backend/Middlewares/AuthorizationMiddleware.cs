using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Attributes;
using backend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace backend.Middlewares;

/// <summary>
/// Middleware kiểm tra quyền chi tiết dựa trên AuthorizeByPermissionAttribute.
/// Luồng xử lý:
/// 1. Lấy endpoint hiện tại từ routing.
/// 2. Kiểm tra endpoint có khai báo quyền chi tiết hay không.
/// 3. Lấy userId từ JWT claim.
/// 4. Đối chiếu PermissionIds (MaQuyen) với dữ liệu VaiTroQuyen + Quyen.
/// 5. Từ chối bằng 403 nếu người dùng không đủ quyền.
/// </summary>
public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationMiddleware> _logger;

    public AuthorizationMiddleware(
        RequestDelegate next,
        ILogger<AuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, PermissionHelper permissionHelper)
    {
        if (HttpMethods.IsOptions(context.Request.Method))
        {
            await _next(context);
            return;
        }

        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            // Kiểm tra endpoint có khai báo quyền chi tiết hay không.
            var authAttribute = endpoint.Metadata.GetMetadata<AuthorizeByPermissionAttribute>();

            if (authAttribute != null)
            {
                var result = await CheckPermissionAsync(context, authAttribute, permissionHelper);
                if (!result)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        message = "Bạn không có quyền truy cập tài nguyên này.",
                        statusCode = 403
                    });
                    return;
                }
            }
        }

        await _next(context);
    }

    private async Task<bool> CheckPermissionAsync(
        HttpContext context,
        AuthorizeByPermissionAttribute authAttribute,
        PermissionHelper permissionHelper)
    {
        try
        {
            // Lấy userId từ JWT token.
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return false;
            }

            // Nếu khai báo quyền bằng chuỗi mã quyền (ví dụ: "PAGE_PARENT_SCHEDULE_VIEW")
            if (!string.IsNullOrWhiteSpace(authAttribute.PermissionCode))
            {
                return await permissionHelper.HasPermissionAsync(
                    userId,
                    authAttribute.MaChucNang,
                    authAttribute.PermissionCode
                );
            }

            if (authAttribute.PermissionIds != null && authAttribute.PermissionIds.Length > 0)
            {
                foreach (var permissionId in authAttribute.PermissionIds)
                {
                    var hasPermission = await permissionHelper.HasPermissionByIdAsync(
                        userId,
                        authAttribute.MaChucNang,
                        permissionId
                    );

                    if (!hasPermission)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Loi khi kiem tra quyen truy cap cho endpoint {Path}", context.Request.Path);
            return false;
        }
    }
}

/// <summary>
/// Extension method để thêm AuthorizationMiddleware vào pipeline cho gọn hơn.
/// </summary>
public static class AuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationMiddleware>();
    }
}
