using System.Collections.Concurrent;

namespace backend.Middlewares;

/// <summary>
/// Giới hạn tần suất gọi endpoint login theo IP để giảm brute-force.
/// </summary>
public class LoginRateLimitMiddleware
{
    private sealed class AttemptWindow
    {
        public int Count { get; set; }
        public DateTime WindowStartUtc { get; set; }
    }

    private static readonly ConcurrentDictionary<string, AttemptWindow> AttemptByIp = new();
    private const int MaxAttempts = 8;
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(5);

    private readonly RequestDelegate _next;

    public LoginRateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Chỉ áp dụng rate limit cho login.
        var isLogin = context.Request.Path.Equals("/api/Auth/login", StringComparison.OrdinalIgnoreCase)
                      && HttpMethods.IsPost(context.Request.Method);

        if (!isLogin)
        {
            await _next(context);
            return;
        }

        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        var window = AttemptByIp.AddOrUpdate(
            ip,
            _ => new AttemptWindow { Count = 1, WindowStartUtc = now },
            (_, existing) =>
            {
                if (now - existing.WindowStartUtc > Window)
                {
                    existing.Count = 1;
                    existing.WindowStartUtc = now;
                }
                else
                {
                    existing.Count += 1;
                }

                return existing;
            });

        if (window.Count > MaxAttempts)
        {
            // Trả về response chuẩn JSON để FE hiển thị theo code lỗi.
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = "Bạn đã thử đăng nhập quá nhiều lần. Vui lòng thử lại sau.",
                errorCode = "AUTH_RATE_LIMIT"
            });
            return;
        }

        await _next(context);
    }
}
