using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.ViewModels;

public class CreateTaskRequest
{
    [Required(ErrorMessage = "Укажите название задачи")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите дату и время")]
    public DateTime ScheduledAt { get; set; }

    [Required(ErrorMessage = "Укажите статус")]
    [AllowedStatus]
    public string Status { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Range(0, 100000, ErrorMessage = "Длительность от 0 до 100000 минут")]
    public int DurationMinutes { get; set; }
}

public class UpdateTaskRequest
{
    [Required(ErrorMessage = "Укажите название задачи")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите дату и время")]
    public DateTime ScheduledAt { get; set; }

    [Required(ErrorMessage = "Укажите статус")]
    [AllowedStatus]
    public string Status { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Range(0, 100000, ErrorMessage = "Длительность от 0 до 100000 минут")]
    public int DurationMinutes { get; set; }
}

public class PatchTaskRequest
{
    public string? Title { get; set; }
    public DateTime? ScheduledAt { get; set; }

    [AllowedStatus]
    public string? Status { get; set; }

    public string? Description { get; set; }

    [Range(0, 100000, ErrorMessage = "Длительность от 0 до 100000 минут")]
    public int? DurationMinutes { get; set; }
}
