using Microsoft.AspNetCore.Mvc;
using TaskScheduler.Models;
using TaskScheduler.Services;
using TaskScheduler.ViewModels;

namespace TaskScheduler.Controllers.Api.V2;

[Route("api/v2/tasks")]
public class TasksV2Controller : Controller
{
    private readonly ITaskRepository _repo;

    public TasksV2Controller(ITaskRepository repo) => _repo = repo;

    [HttpGet("")]
    public async Task<IActionResult> List([FromQuery] string? status, CancellationToken ct)
    {
        var items = string.IsNullOrWhiteSpace(status)
            ? await _repo.GetAllAsync(ct)
            : await _repo.GetByStatusAsync(status, ct);

        return Json(items.Select(ToDtoV2));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        if (task is null)
            return NotFound(new { error = "Task not found", id });

        return Json(ToDtoV2(task));
    }

    private static TaskDtoV2 ToDtoV2(ScheduledTask t)
        => new(t.Id, t.Title, t.ScheduledAt, t.Status, t.Description, t.DurationMinutes);
}
