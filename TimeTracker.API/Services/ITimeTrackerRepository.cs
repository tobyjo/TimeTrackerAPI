using TimeTracker.API.Entities;

namespace TimeTracker.API.Services
{
    public interface ITimeTrackerRepository
    {
        Task<IEnumerable<TimeEntry>> GetTimeEntriesAsync();

        Task<TimeEntry?> GetTimeEntryAsync(int timeEntryId, bool includeChildren);

        Task<IEnumerable<TimeEntry>> GetTimeEntriesForUserAsync(int userId, bool includeChildren);

        Task<IEnumerable<TimeEntry>> GetTimeEntriesForUserWithDateRangeAsync(int userId, DateTime startDateTime, DateTime endDateTime, bool includeChildren);
    }
}
