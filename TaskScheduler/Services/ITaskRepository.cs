using TaskScheduler.Models;

namespace TaskScheduler.Services;

public interface ITaskRepository
{
    Task<IReadOnlyList<ScheduledTask>> GetAllAsync(CancellationToken ct = default);
    Task<ScheduledTask?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ScheduledTask>> GetByStatusAsync(string status, CancellationToken ct = default);
}
