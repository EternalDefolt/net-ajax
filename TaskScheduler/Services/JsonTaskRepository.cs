using System.Text.Json;
using TaskScheduler.Models;

namespace TaskScheduler.Services;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

    public JsonTaskRepository(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "Data", "tasks.json");
    }

    private async Task<List<ScheduledTask>> LoadAsync(CancellationToken ct)
    {
        if (!File.Exists(_filePath)) return new();
        await using var stream = File.OpenRead(_filePath);
        var items = await JsonSerializer.DeserializeAsync<List<ScheduledTask>>(stream, _opts, ct);
        return items ?? new();
    }

    public async Task<IReadOnlyList<ScheduledTask>> GetAllAsync(CancellationToken ct = default)
        => await LoadAsync(ct);

    public async Task<ScheduledTask?> GetByIdAsync(int id, CancellationToken ct = default)
        => (await LoadAsync(ct)).FirstOrDefault(t => t.Id == id);

    public async Task<IReadOnlyList<ScheduledTask>> GetByStatusAsync(string status, CancellationToken ct = default)
        => (await LoadAsync(ct))
            .Where(t => string.Equals(t.Status, status, StringComparison.OrdinalIgnoreCase))
            .ToList();
}
