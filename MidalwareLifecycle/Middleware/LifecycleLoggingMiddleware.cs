using System.Diagnostics;

namespace MidalwareLifecycle.Middleware;

public class LifecycleLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LifecycleLoggingMiddleware> _logger;

    public LifecycleLoggingMiddleware(RequestDelegate next, ILogger<LifecycleLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        _logger.LogInformation("[1] Запрос вошёл в конвейер:  {Method} {Path}",
            context.Request.Method, context.Request.Path);

        _logger.LogInformation("[2] Передаю управление дальше по конвейеру -> next()");
        await _next(context);
        _logger.LogInformation("[3] Управление вернулось из конвейера <- next()");

        sw.Stop();
        _logger.LogInformation("[4] Ответ уходит клиенту: {Status}, обработано за {Ms} мс",
            context.Response.StatusCode, sw.ElapsedMilliseconds);
    }
}
