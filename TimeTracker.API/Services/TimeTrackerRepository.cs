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

        public async Task<Team?> GetTeamAsync(int teamId, bool includeUsers)
        {
            if( includeUsers)
            {   var teamWithUsers = await _context.Teams
                    .Include(t => t.Users)
                    .Where(t => t.Id == teamId).FirstOrDefaultAsync();
                return teamWithUsers;
            }
            else
            {
                var teamWithoutUsers = await _context.Teams
                    .Where(t => t.Id == teamId).FirstOrDefaultAsync();
                return teamWithoutUsers;
            }
        }

        public async Task AddTeamAsync(Team team)
        {

            // Add team to context
            await _context.Teams.AddAsync(team);

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
