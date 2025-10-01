using TimeTracker.API.Entities;

namespace TimeTracker.API.Services
{
    public interface ITimeTrackerRepository
    {
        Task<IEnumerable<Team>> GetTeamsAsync();

        Task<Team?> GetTeamAsync(int teamId, bool includeUsers, bool includeProjects);

        Task AddTeamAsync( Team team);


        Task<Project?> GetProjectAsync(int projectId);

        Task AddProjectAsync(Project project);

        Task<User?> GetUserAsync(int userId );

        Task<User?> GetUserWithTimeEntriesAsync(int userId);

        Task<User?> GetUserWithProjectsAsync(int userId);

        Task<TimeEntry?> GetTimeEntryAsync(int timeEntryId);

        Task<bool> SaveChangesAsync();

        Task<IEnumerable<TimeEntry>> GetTimeEntriesAsync();

        Task<TimeEntry?> GetTimeEntryAsync(int timeEntryId, bool includeChildren);

        Task<IEnumerable<TimeEntry>> GetTimeEntriesForUserAsync(int userId, bool includeChildren);

        Task<IEnumerable<TimeEntry>> GetTimeEntriesForUserWithDateRangeAsync(int userId, DateTime startDateTime, DateTime endDateTime, bool includeChildren);
    }
}
