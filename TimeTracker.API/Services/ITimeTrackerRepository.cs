using System.Threading.Tasks;
using TimeTracker.API.Entities;
using System;
using System.Collections.Generic;

namespace TimeTracker.API.Services
{
    public interface ITimeTrackerRepository
    {
        Task<IEnumerable<Team>> GetTeamsAsync();

        Task<Team?> GetTeamAsync(int teamId, bool includeUsers, bool includeProjects);

        Task AddTeamAsync( Team team);


        Task<Project?> GetProjectAsync(int projectId);

        Task AddProjectAsync(Project project);

        Task<User?> GetUserAsync(string userId );

        Task AddUserAsync(User newUser);

        Task<bool> UserExistsAsync(string userId);

        Task<User?> GetUserWithTimeEntriesAsync(string userId);

        Task<User?> GetUserWithTimeEntriesWithDateRangeAsync( string userId, DateTime startDateTime, DateTime endDateTime);

        Task<User?> GetUserWithProjectsAsync(string userId);

        Task<User?> GetUserWithSegmentTypesAsync(string userId);

        Task<TimeEntry?> GetTimeEntryAsync(int timeEntryId);

        Task AddTimeEntryAsync(TimeEntry timeEntry);

        void DeleteTimeEntry(TimeEntry timeEntry);

        Task<SegmentType?> GetSegmentTypeAsync(int segmentTypeId);

        Task<bool> SaveChangesAsync();

        Task<IEnumerable<TimeEntry>> GetTimeEntriesAsync();

        Task<TimeEntry?> GetTimeEntryAsync(int timeEntryId, bool includeChildren);

        Task<IEnumerable<TimeEntry>> GetTimeEntriesForUserAsync(string userId, bool includeChildren);

    }
}
