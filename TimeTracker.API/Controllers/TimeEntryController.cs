using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Services;
using TimeTracker.API.Models;
using AutoMapper;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/timeentry")]
    public class TimeEntryController : ControllerBase
    {
        private readonly ITimeTrackerRepository timeTrackerRepository;
        private readonly IMapper mapper;

        public TimeEntryController(ITimeTrackerRepository timeTrackerRepository, IMapper mapper)
        {
            this.timeTrackerRepository = timeTrackerRepository ?? throw new ArgumentNullException(nameof(timeTrackerRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //[HttpGet]
        //public IEnumerable<TimeEntryDto> Get() => _store.Data.TimeEntry;

        // TODO: Rewrite from a user persepctive i.e.  api/user/1/timeentries 
        // https://localhost:7201/api/timeentry?userId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeEntryWithDetailsDto>>> Get([FromQuery] int userId)
        {
            //var entries = _store.Data.TimeEntry;
            var entries = await timeTrackerRepository.GetTimeEntriesForUserAsync(userId, true);
            return Ok(mapper.Map<IEnumerable<TimeEntryWithDetailsDto>>(entries));

            /*
            var results = new List<TimeEntryWithDetailsDto>();
            foreach(var entry in entries)
            {
                results.Add(new TimeEntryWithDetailsDto
                {
                    StartDateTime = entry.StartDateTime,
                    EndDateTime = entry.EndDateTime,
                    UserID = entry.UserId,
                    ProjectCode = entry.Project.Code,
                    ProjectDescription = entry.Project.Description,
                    SegmentTypeName = entry.SegmentType.Name
                });
            }
               return Ok(results);
            */

        }

    }
}