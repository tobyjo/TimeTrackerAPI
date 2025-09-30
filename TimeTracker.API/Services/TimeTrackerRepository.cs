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

        public Task<Project?> GetProjectAsync(int projectId)
        {
            return _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task AddProjectAsync(Project project)
        {

            // Add team to context
            await _context.Projects.AddAsync(project);

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

        public Task<IEnumerable<Entities.TimeEntry>> GetTimeEntriesForUserWithDateRangeAsync(int userId, DateTime startDateTime, DateTime endDateTime, bool includeChildren)
        {
            throw new NotImplementedException();
        }




   
    }
}
