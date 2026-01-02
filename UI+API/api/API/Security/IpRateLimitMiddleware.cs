using System.Collections.Concurrent;

namespace API.Security
{
    /// <summary>
    /// Rate limit đơn giản theo IP cho endpoint login (chống brute force).
    /// Mặc định: 30 requests / 60 giây / IP cho /api/Auth/*login
    /// </summary>
    public class IpRateLimitMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly int _limit;
        private readonly TimeSpan _window;

        private static readonly ConcurrentDictionary<string, Counter> _counters = new();

        private sealed class Counter
        {
            public DateTime WindowStartUtc { get; set; }
            public int Count { get; set; }
            public object LockObj { get; } = new object();
        }

        public IpRateLimitMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;

            _limit = config.GetValue("Security:LoginRateLimit:Limit", 30);
            var seconds = config.GetValue("Security:LoginRateLimit:WindowSeconds", 60);
            _window = TimeSpan.FromSeconds(seconds);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";

            // Chỉ áp dụng cho login
            if (path.StartsWith("/api/Auth/", StringComparison.OrdinalIgnoreCase) &&
                (path.EndsWith("admin-login", StringComparison.OrdinalIgnoreCase) ||
                 path.EndsWith("employee-login", StringComparison.OrdinalIgnoreCase)))
            {
                var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var now = DateTime.UtcNow;

                var counter = _counters.GetOrAdd(ip, _ => new Counter
                {
                    WindowStartUtc = now,
                    Count = 0
                });

                bool overLimit = false;

                lock (counter.LockObj)
                {
                    if (now - counter.WindowStartUtc > _window)
                    {
                        counter.WindowStartUtc = now;
                        counter.Count = 0;
                    }

                    counter.Count++;

                    if (counter.Count > _limit)
                        overLimit = true;
                }

                if (overLimit)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"message\":\"Too many login attempts. Please try again later.\"}");
                    return; 
                }
            }

            await _next(context);
        }
    }
}
