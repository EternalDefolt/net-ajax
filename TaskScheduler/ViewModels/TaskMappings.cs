using TaskScheduler.Models;

namespace TaskScheduler.ViewModels;

public static class TaskMappings
{
    public static TaskDtoV2 ToResponse(this ScheduledTask t)
        => new(t.Id, t.Title, t.ScheduledAt, t.Status, t.Description, t.DurationMinutes);

    public static ScheduledTask ToEntity(this CreateTaskRequest r)
        => new()
        {
            Title = r.Title,
            ScheduledAt = r.ScheduledAt,
            Status = r.Status,
            Description = r.Description,
            DurationMinutes = r.DurationMinutes
        };

    public static void Apply(this UpdateTaskRequest r, ScheduledTask t)
    {
        t.Title = r.Title;
        t.ScheduledAt = r.ScheduledAt;
        t.Status = r.Status;
        t.Description = r.Description;
        t.DurationMinutes = r.DurationMinutes;
    }

    public static void Apply(this PatchTaskRequest r, ScheduledTask t)
    {
        if (r.Title is not null) t.Title = r.Title;
        if (r.ScheduledAt is not null) t.ScheduledAt = r.ScheduledAt.Value;
        if (r.Status is not null) t.Status = r.Status;
        if (r.Description is not null) t.Description = r.Description;
        if (r.DurationMinutes is not null) t.DurationMinutes = r.DurationMinutes.Value;
    }
}
