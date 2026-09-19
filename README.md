# net.ajax

Работы по предмету net.ajax (ASP.NET Core).

## Работа 1. Middleware и жизненный цикл приложения

Проект в папке `MidalwareLifecycle`. Сделан из пустого шаблона `dotnet new web`.
В приложении есть форма и своё middleware, через него видно весь жизненный цикл.

### Жизненный цикл хоста

В `Program.cs` через `IHostApplicationLifetime`:

- `ApplicationStarted` - приложение запустилось
- `ApplicationStopping` - началась остановка
- `ApplicationStopped` - приложение остановилось

### Жизненный цикл запроса (конвейер middleware)

1. Своё middleware `LifecycleLoggingMiddleware`, запрос входит в конвейер
2. `next()`, управление идёт дальше
3. Inline-middleware, ставит заголовок ответа
4. Эндпоинт `POST /submit`, обработка формы
5. Возврат управления обратно
6. Ответ клиенту, замер времени

### Запуск

```
cd MidalwareLifecycle
dotnet run
```

Открыть адрес из консоли, заполнить форму, отправить. В консоли идёт лог всего цикла.
Останавливать через Ctrl+C, тогда пишутся ApplicationStopping и ApplicationStopped.

Скрины в папке `screenshots`.
