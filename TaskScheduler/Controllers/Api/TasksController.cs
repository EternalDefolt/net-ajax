using Microsoft.AspNetCore.Mvc;
using TaskScheduler.Models;
using TaskScheduler.Services;
using TaskScheduler.ViewModels;

namespace TaskScheduler.Controllers.Api;

[Route("api/tasks")]
public class TasksController : Controller
{
    private readonly ITaskRepository _repo;

    public TasksController(ITaskRepository repo) => _repo = repo;

    [HttpGet("")]
    public async Task<IActionResult> List([FromQuery] string? status, CancellationToken ct)
    {
        var items = string.IsNullOrWhiteSpace(status)
            ? await _repo.GetAllAsync(ct)
            : await _repo.GetByStatusAsync(status, ct);

        return Json(items.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        if (task is null)
            return NotFound(new { error = "Task not found", id });

        return Json(ToDto(task));
    }

    private static TaskDto ToDto(ScheduledTask t)
        => new(t.Id, t.Title, t.ScheduledAt, t.Status);
}
