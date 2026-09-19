using TaskScheduler.Models;

namespace TaskScheduler.Services;

public interface ITaskRepository
{
    Task<IReadOnlyList<ScheduledTask>> GetAllAsync(CancellationToken ct = default);
    Task<ScheduledTask?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ScheduledTask>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<ScheduledTask> AddAsync(ScheduledTask task, CancellationToken ct = default);
    Task<bool> UpdateAsync(ScheduledTask task, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
