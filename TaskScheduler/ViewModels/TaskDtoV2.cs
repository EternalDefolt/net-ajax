namespace TaskScheduler.ViewModels;

public record TaskDtoV2(
    int Id,
    string Title,
    DateTime ScheduledAt,
    string Status,
    string Description,
    int DurationMinutes);
