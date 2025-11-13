using System.Collections.Concurrent;

namespace TimeTracker.API.Services;

public interface IInMemoryLogger
{
 void Log(string message);
    IEnumerable<string> GetRecentLogs(int count = 50);
    void Clear();
}

public class InMemoryLogger : IInMemoryLogger
{
    // Using ConcurrentQueue for thread-safety
    private readonly ConcurrentQueue<string> _logs = new();
    private const int MaxLogCount = 1000; // Limit memory usage

    public void Log(string message)
    {
        var timestampedMessage = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - {message}";
        _logs.Enqueue(timestampedMessage);

        // Remove old logs if we exceed the limit
        while (_logs.Count > MaxLogCount)
        {
            _logs.TryDequeue(out _);
        }
    }

    public IEnumerable<string> GetRecentLogs(int count = 50)
    {
        return _logs.TakeLast(count).ToList();
    }

    public void Clear()
    {
        _logs.Clear();
    }
}
