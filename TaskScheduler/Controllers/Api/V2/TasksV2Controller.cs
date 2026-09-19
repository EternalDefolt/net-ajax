using Microsoft.AspNetCore.Mvc;
using TaskScheduler.Infrastructure;
using TaskScheduler.Services;
using TaskScheduler.ViewModels;

namespace TaskScheduler.Controllers.Api.V2;

[ApiController]
[Route("api/v2/tasks")]
public class TasksV2Controller : ControllerBase
{
    private readonly ITaskRepository _repo;
    private readonly IIdempotencyStore _idempotency;

    public TasksV2Controller(ITaskRepository repo, IIdempotencyStore idempotency)
    {
        _repo = repo;
        _idempotency = idempotency;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDtoV2>>> List([FromQuery] string? status, CancellationToken ct)
    {
        var items = string.IsNullOrWhiteSpace(status)
            ? await _repo.GetAllAsync(ct)
            : await _repo.GetByStatusAsync(status, ct);

        return Ok(items.Select(t => t.ToResponse()));
    }

    [HttpGet("by-ids")]
    public async Task<ActionResult<IEnumerable<TaskDtoV2>>> GetByIds(
        [ModelBinder(typeof(CsvIntArrayBinder))] int[] ids, CancellationToken ct)
    {
        var wanted = ids.ToHashSet();
        var items = await _repo.GetAllAsync(ct);
        return Ok(items.Where(t => wanted.Contains(t.Id)).Select(t => t.ToResponse()));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskDtoV2>> GetOne(
        int id, [FromHeader(Name = "If-None-Match")] string? ifNoneMatch, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        if (task is null) return NotFoundProblem(id);

        var etag = ETagHelper.Compute(task);
        if (!string.IsNullOrEmpty(ifNoneMatch) && ifNoneMatch == etag)
            return StatusCode(StatusCodes.Status304NotModified);

        Response.Headers.ETag = etag;
        return Ok(task.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<TaskDtoV2>> Create(
        [FromBody] CreateTaskRequest request,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
        CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(idempotencyKey) && _idempotency.TryGet(idempotencyKey, out var existingId))
        {
            var existing = await _repo.GetByIdAsync(existingId, ct);
            if (existing is not null)
            {
                Response.Headers.ETag = ETagHelper.Compute(existing);
                return Ok(existing.ToResponse());
            }
        }

        var created = await _repo.AddAsync(request.ToEntity(), ct);

        if (!string.IsNullOrWhiteSpace(idempotencyKey))
            _idempotency.Save(idempotencyKey, created.Id);

        Response.Headers.ETag = ETagHelper.Compute(created);
        return CreatedAtAction(nameof(GetOne), new { id = created.Id }, created.ToResponse());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskDtoV2>> Replace(
        int id,
        [FromBody] UpdateTaskRequest request,
        [FromHeader(Name = "If-Match")] string? ifMatch,
        CancellationToken ct)
    {
        var current = await _repo.GetByIdAsync(id, ct);
        if (current is null) return NotFoundProblem(id);

        if (string.IsNullOrWhiteSpace(ifMatch))
            return Problem(
                statusCode: StatusCodes.Status428PreconditionRequired,
                title: "Precondition Required",
                detail: "Для изменения нужен заголовок If-Match с текущим ETag");

        if (ifMatch != ETagHelper.Compute(current))
            return PreconditionFailed();

        request.Apply(current);
        await _repo.UpdateAsync(current, ct);

        Response.Headers.ETag = ETagHelper.Compute(current);
        return Ok(current.ToResponse());
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<TaskDtoV2>> Patch(
        int id,
        [FromBody] PatchTaskRequest request,
        [FromHeader(Name = "If-Match")] string? ifMatch,
        CancellationToken ct)
    {
        var current = await _repo.GetByIdAsync(id, ct);
        if (current is null) return NotFoundProblem(id);

        if (!string.IsNullOrWhiteSpace(ifMatch) && ifMatch != ETagHelper.Compute(current))
            return PreconditionFailed();

        request.Apply(current);
        await _repo.UpdateAsync(current, ct);

        Response.Headers.ETag = ETagHelper.Compute(current);
        return Ok(current.ToResponse());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await _repo.DeleteAsync(id, ct);
        if (!deleted) return NotFoundProblem(id);
        return NoContent();
    }

    private ObjectResult NotFoundProblem(int id)
        => Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Task not found",
            detail: $"Задача с id {id} не найдена",
            type: "https://example.com/errors/not-found",
            instance: HttpContext.Request.Path);

    private ObjectResult PreconditionFailed()
        => Problem(
            statusCode: StatusCodes.Status412PreconditionFailed,
            title: "Precondition Failed",
            detail: "ETag не совпадает: задачу изменил кто-то другой");
}
