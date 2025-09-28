using Microsoft.Extensions.Logging;
using System.Diagnostics;

public class ResponseTimeLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponseTimeLoggingMiddleware> _logger;

    public ResponseTimeLoggingMiddleware(RequestDelegate next, ILogger<ResponseTimeLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        // Use OnStarting to add headers BEFORE response starts
        context.Response.OnStarting(() =>
        {
            stopwatch.Stop();
            var responseTime = stopwatch.ElapsedMilliseconds;
            var responseTimeSeconds = responseTime / 1000.0;
            // Only add header if response hasn't started yet
            if (!context.Response.HasStarted)
            {
                context.Response.Headers.TryAdd("X-Response-Time-s", responseTimeSeconds.ToString());
            }
            
            // Log to console
            _logger.LogInformation("Request {Method} {Path} completed in {responseTimeSeconds}s with status {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                responseTimeSeconds.ToString(),
                context.Response.StatusCode);
                
            return Task.CompletedTask;
        });

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Request {Method} {Path} failed in {ResponseTime}ms", 
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
