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
