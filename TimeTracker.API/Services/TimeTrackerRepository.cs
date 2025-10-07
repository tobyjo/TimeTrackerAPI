using Microsoft.EntityFrameworkCore;
using TimeTracker.API.DbContexts;
using TimeTracker.API.Entities;

namespace TimeTracker.API.Services
{
    public class TimeTrackerRepository : ITimeTrackerRepository
    {

        private readonly TimeTrackerContext _context;
        public TimeTrackerRepository(TimeTrackerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            return await _context.Teams.OrderBy(t => t.TeamName).ToListAsync();
        }

        public async Task<Team?> GetTeamAsync(int teamId, bool includeUsers, bool includeProjects)
        {
            IQueryable<Team> query = _context.Teams;

            if (includeUsers)
            {
                query = query.Include(t => t.Users);
            }

            if (includeProjects)
            {
                query = query.Include(t => t.Projects);
            }

            return await query.FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task AddTeamAsync(Team team)
        {

            // Add team to context
            await _context.Teams.AddAsync(team);

        }

        public async Task<Project?> GetProjectAsync(int projectId)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task AddProjectAsync(Project project)
        {

            // Add team to context
            await _context.Projects.AddAsync(project);

        }

        public async Task<User?> GetUserAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithTimeEntriesAsync(int userId)
        {
            IQueryable<User> query = _context.Users;

            // Include TimeEntries and their SegmentType and Project details
            query = query
                .Include(u => u.TimeEntries)
                    .ThenInclude(te => te.SegmentType)
                .Include(u => u.TimeEntries)
                    .ThenInclude(te => te.Project);

            return await query.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithTimeEntriesWithDateRangeAsync(int userId, DateTime startDateTime, DateTime endDateTime)
        {
            IQueryable<User> query = _context.Users;
            // Include TimeEntries and their SegmentType and Project details
            query = query
                .Include(u => u.TimeEntries.Where(te => te.StartDateTime >= startDateTime && te.EndDateTime <= endDateTime))
                    .ThenInclude(te => te.SegmentType)
                .Include(u => u.TimeEntries.Where(te => te.StartDateTime >= startDateTime && te.EndDateTime <= endDateTime))
                    .ThenInclude(te => te.Project);
            return await query.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithProjectsAsync(int userId)
        {  
            // User only has one team and the user entity only has a teamId. We use that to get the list of projects for the Team.
            // Include the Team and then the Projects for that Team
            return await _context.Users
                .Include(u => u.Team)
                    .ThenInclude(t => t.Projects)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }


        public async Task<TimeEntry?> GetTimeEntryAsync(int timeEntryId)
        {
            return await _context.TimeEntries
                 .Include(te => te.Project)
                 .Include(te => te.SegmentType)
                 .Include(te => te.User)
                 .FirstOrDefaultAsync(te => te.Id == timeEntryId);
        }

        public async Task AddTimeEntryAsync(TimeEntry timeEntry)
        {
            // Add team to context
            await _context.TimeEntries.AddAsync(timeEntry);
        }

        public async Task<SegmentType?> GetSegmentTypeAsync(int segmentTypeId)
        {
            return await _context.SegmentTypes.FirstOrDefaultAsync(st => st.Id == segmentTypeId);
        }

        public async Task<User?> GetUserWithSegmentTypesAsync(int userId)
        {
            // User only has one team and the user entity only has a teamId. We use that to get the list of segmenttypes for the Team.
            // Include the Team and then the SegmentTypes for that Team
            return await _context.Users
                .Include(u => u.Team)
                    .ThenInclude(t => t.SegmentTypes)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }


        public async Task<IEnumerable<Entities.TimeEntry>> GetTimeEntriesAsync()
        {
            return await _context.TimeEntries.OrderBy(d => d.StartDateTime).ToListAsync();
        }

        public async Task<Entities.TimeEntry?> GetTimeEntryAsync(int timeEntryId, bool includeChildren)
        {
            if (includeChildren)
            {
                return await _context.TimeEntries
                    .Include(te => te.Project)
                    .Include(te => te.SegmentType)
                    .Include(te => te.User)
                    .FirstOrDefaultAsync(te => te.Id == timeEntryId);
            }

            return await _context.TimeEntries
                .FirstOrDefaultAsync(te => te.Id == timeEntryId);
        }
        
        public async Task<IEnumerable<Entities.TimeEntry>> GetTimeEntriesForUserAsync(int userId, bool includeChildren)
        {
            if (includeChildren)
            {
                return await _context.TimeEntries
                    .Include(te => te.Project)
                    .Include(te => te.SegmentType)
                    .Include(te => te.User)
                    .Where(te => te.UserId == userId).ToListAsync();
            }

            return await _context.TimeEntries
                  .Where(te => te.UserId == userId).ToListAsync();
        }
   
    }
}
