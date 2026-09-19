using MidalwareLifecycle.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var lifetime = app.Lifetime;
var log = app.Logger;

lifetime.ApplicationStarted.Register(() =>
    log.LogInformation(">>> ЖЦ ХОСТА: ApplicationStarted, приложение запущено и принимает запросы"));
lifetime.ApplicationStopping.Register(() =>
    log.LogInformation(">>> ЖЦ ХОСТА: ApplicationStopping, приложение начинает остановку"));
lifetime.ApplicationStopped.Register(() =>
    log.LogInformation(">>> ЖЦ ХОСТА: ApplicationStopped, приложение остановлено"));

app.UseMiddleware<LifecycleLoggingMiddleware>();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Handled-By"] = "MidalwareLifecycle";
    app.Logger.LogInformation("[inline] Встроенное middleware поставило заголовок ответа");
    await next();
});

app.MapGet("/", () => Results.Content(FormPage(null), "text/html; charset=utf-8"));

app.MapPost("/submit", async (HttpContext ctx) =>
{
    var form = await ctx.Request.ReadFormAsync();
    var name = form["name"].ToString();
    var message = form["message"].ToString();
    app.Logger.LogInformation("[endpoint] Форма принята: name='{Name}', message='{Message}'", name, message);
    return Results.Content(FormPage((name, message)), "text/html; charset=utf-8");
});

app.Run();

static string FormPage((string name, string message)? result)
{
    var block = result is null
        ? ""
        : $"""
          <div class="result">
            <h2>Форма обработана конвейером</h2>
            <p><b>Имя:</b> {System.Net.WebUtility.HtmlEncode(result.Value.name)}</p>
            <p><b>Сообщение:</b> {System.Net.WebUtility.HtmlEncode(result.Value.message)}</p>
          </div>
          """;

    return $$"""
    <!doctype html>
    <html lang="ru">
    <head>
      <meta charset="utf-8">
      <meta name="viewport" content="width=device-width, initial-scale=1">
      <title>ASP.NET Middleware, жизненный цикл</title>
      <style>
        body { font-family: system-ui, sans-serif; max-width: 640px; margin: 40px auto; padding: 0 16px; }
        form { display: grid; gap: 12px; }
        input, textarea { padding: 8px; font-size: 15px; }
        button { padding: 10px; font-size: 15px; cursor: pointer; }
        .result { margin-top: 24px; padding: 16px; border: 1px solid #ccc; border-radius: 8px; }
      </style>
    </head>
    <body>
      <h1>Форма обратной связи</h1>
      <p>Форма отправляется через конвейер middleware. Лог видно в консоли.</p>
      <form method="post" action="/submit">
        <input name="name" placeholder="Имя" required>
        <textarea name="message" placeholder="Сообщение" rows="4" required></textarea>
        <button type="submit">Отправить</button>
      </form>
      {{block}}
    </body>
    </html>
    """;
}
