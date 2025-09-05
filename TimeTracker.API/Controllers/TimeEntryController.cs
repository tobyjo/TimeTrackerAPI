using Microsoft.AspNetCore.Mvc;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/timeentry")]
    public class TimeEntryController : ControllerBase
    {
        private readonly TimeTrackerDataStore _store;

        public TimeEntryController(TimeTrackerDataStore store)
        {
            _store = store;
        }

        //[HttpGet]
        //public IEnumerable<TimeEntryDto> Get() => _store.Data.TimeEntry;

        [HttpGet]
        public ActionResult<IEnumerable<TimeEntryWithDetailsDto>> Get([FromQuery] int? userId)
        {
            var entries = _store.Data.TimeEntry;
            if (userId.HasValue)
                entries = entries.Where(e => e.UserID == userId.Value).ToList();

        

            var result = entries.Select(e =>
            {
                // Find the matching project by id
                var project = _store.Data.Project.FirstOrDefault(p => p.ID == e.ProjectID);

                // Find the matching segment type by id
                var segmentType = _store.Data.SegmentType.FirstOrDefault(s => s.ID == e.SegmentTypeID);


                return new TimeEntryWithDetailsDto
                {
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime,
                    UserID = e.UserID,
                    ProjectCode = project?.Code ?? string.Empty,
                    ProjectDescription = project?.Description ?? string.Empty,
                    SegmentTypeName = segmentType?.Name ?? string.Empty
                };
            }).ToList();

            return Ok(result);
        }
        
    }
}