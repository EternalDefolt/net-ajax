# Планировщик задач (API)

Архитектура API на ASP.NET Core (MVC). Тема: планировщик задач. Данные читаются
из `Data/tasks.json` через репозиторий. API отдаёт две версии ответа: v1 и v2.

## Слои

- `Models/ScheduledTask.cs` - модель задачи со всеми полями
- `Services/ITaskRepository.cs`, `Services/JsonTaskRepository.cs` - чтение задач из json
- `ViewModels/TaskDto.cs` - ответ v1
- `ViewModels/TaskDtoV2.cs` - ответ v2 (поля v1 плюс два новых)
- `Controllers/Api/TasksController.cs` - API v1
- `Controllers/Api/V2/TasksV2Controller.cs` - API v2
- `Controllers/HomeController.cs` + `Views/Home/Index.cshtml` - список задач и ссылки на API

## Расписание версий

| Версия | Маршрут | Поля в ответе |
| --- | --- | --- |
| v1 | `GET /api/tasks`, `GET /api/tasks/{id}` | Id, Title, ScheduledAt, Status |
| v2 | `GET /api/v2/tasks`, `GET /api/v2/tasks/{id}` | те же плюс Description, DurationMinutes |

v1 не меняется. v2 добавляет два поля. Старые записи в `tasks.json` без этих
полей читаются без ошибок: Description пустое, DurationMinutes равно 0.

Фильтр по статусу: `GET /api/tasks?status=planned` (есть и в v2).

## Скрины

- `TaskScheduler/screenshots/01-home.jpg` - список задач и ссылки на API
- `TaskScheduler/screenshots/02-api-v1.jpg` - ответ v1, четыре поля
- `TaskScheduler/screenshots/03-api-v2.jpg` - ответ v2, шесть полей

## Запуск

```
cd TaskScheduler
dotnet run
```
