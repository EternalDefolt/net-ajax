using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.ViewModels;

public class AllowedStatusAttribute : ValidationAttribute
{
    public static readonly string[] Allowed = { "planned", "in_progress", "done", "canceled" };

    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        if (value is null) return ValidationResult.Success;

        var status = value.ToString();
        if (Allowed.Contains(status, StringComparer.OrdinalIgnoreCase))
            return ValidationResult.Success;

        return new ValidationResult(
            $"Статус должен быть одним из: {string.Join(", ", Allowed)}",
            new[] { ctx.MemberName! });
    }
}
