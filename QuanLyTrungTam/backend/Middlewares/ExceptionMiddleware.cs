using backend.DTOs;

namespace backend.Middlewares;

/// <summary>
/// Bọc lỗi toàn cục để ẩn chi tiết kỹ thuật với người dùng nhưng vẫn log đủ cho dev.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var errorId = Guid.NewGuid().ToString("N");
            _logger.LogError(ex, "Unhandled error {ErrorId} at {Path}", errorId, context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponseDto<object>.Fail(
                "Đã có lỗi hệ thống. Vui lòng thử lại sau.",
                "SERVER_ERROR");

            response.Data = new { errorId };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
