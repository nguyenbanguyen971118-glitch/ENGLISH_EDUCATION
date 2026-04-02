namespace backend.Middlewares;

/// <summary>
/// Ghi log sự kiện auth (login/refresh) phục vụ audit và theo dõi vận hành.
/// Người tạo: nmkhue
/// Ngày tạo: 28/3
/// </summary>
public class LoginAuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoginAuditMiddleware> _logger;

    public LoginAuditMiddleware(RequestDelegate next, ILogger<LoginAuditMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Chỉ audit cho các endpoint auth có ghi trạng thái đăng nhập.
        var isAuthWrite = (context.Request.Path.Equals("/api/Auth/login", StringComparison.OrdinalIgnoreCase)
                          || context.Request.Path.Equals("/api/Auth/refresh", StringComparison.OrdinalIgnoreCase))
                          && HttpMethods.IsPost(context.Request.Method);

        if (!isAuthWrite)
        {
            await _next(context);
            return;
        }

        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var start = DateTime.UtcNow;

        await _next(context);

        // Log sau khi xử lý để lấy được status code thực tế.
        var elapsedMs = (DateTime.UtcNow - start).TotalMilliseconds;
        var level = context.Response.StatusCode < 400 ? LogLevel.Information : LogLevel.Warning;

        _logger.Log(level,
            "Auth event {Method} {Path} status={StatusCode} ip={Ip} durationMs={Duration}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            ip,
            Math.Round(elapsedMs, 2));
    }
}
