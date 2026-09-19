using System.Collections.Concurrent;

namespace TaskScheduler.Services;

public interface IIdempotencyStore
{
    bool TryGet(string key, out int taskId);
    void Save(string key, int taskId);
}

public class InMemoryIdempotencyStore : IIdempotencyStore
{
    private static readonly TimeSpan Ttl = TimeSpan.FromHours(24);
    private readonly ConcurrentDictionary<string, (DateTime Expires, int TaskId)> _map = new();

    public bool TryGet(string key, out int taskId)
    {
        taskId = 0;
        if (_map.TryGetValue(key, out var entry))
        {
            if (entry.Expires > DateTime.UtcNow)
            {
                taskId = entry.TaskId;
                return true;
            }
            _map.TryRemove(key, out _);
        }
        return false;
    }

    public void Save(string key, int taskId)
        => _map[key] = (DateTime.UtcNow.Add(Ttl), taskId);
}
