namespace TaskScheduler.ViewModels;

public record TaskDto(int Id, string Title, DateTime ScheduledAt, string Status);
