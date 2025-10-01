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

        [HttpGet("{id}", Name = "GetTimeEntry")]
        public async Task<IActionResult> GetTimeEntry(int id)
        {
            var timeEntry = await timeTrackerRepository.GetTimeEntryAsync(id);
            if (timeEntry == null)
                return NotFound();

            var timeEntryResult = mapper.Map<TimeEntryWithDetailsDto>(timeEntry);
            return Ok(timeEntryResult);

        }

        [HttpPost]
        public async Task<ActionResult<TimeEntryDto>> CreateTimeEntry(TimeEntryForCreationDto timeEntry)
        {
            // map to entity
            var timeEntryEntity = mapper.Map<Entities.TimeEntry>(timeEntry);

            await timeTrackerRepository.AddTimeEntryAsync(timeEntryEntity);
            await timeTrackerRepository.SaveChangesAsync();

            // return the created team
            var createdTimeEntryToReturn = mapper.Map<TimeEntryDto>(timeEntryEntity);
            return CreatedAtRoute("GetTimeEntry",
                new { id = createdTimeEntryToReturn.Id },
                createdTimeEntryToReturn);
        }

    }
}