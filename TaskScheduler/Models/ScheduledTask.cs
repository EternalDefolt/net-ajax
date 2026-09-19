using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.Models
{
    public class ScheduledTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите название задачи")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите дату и время")]
        public DateTime ScheduledAt { get; set; }

        [Required(ErrorMessage = "Укажите статус")]
        public string Status { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0, 100000, ErrorMessage = "Длительность от 0 до 100000 минут")]
        public int DurationMinutes { get; set; }
    }
}
