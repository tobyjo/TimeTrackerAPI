using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Services;
using TimeTracker.API.Models;
using AutoMapper;
using System.Runtime.CompilerServices;

namespace TimeTracker.API.Controllers
{
    // The controller should really be api/users/{userId}/timeentries but we are doing that in Users controller
    [ApiController]
    [Route("api/users/{userId}/timeentries")]
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
        public async Task<ActionResult<TimeEntryDto>> CreateTimeEntry(int userId, [FromBody] TimeEntryForCreationDto timeEntry)
        {
            // map to entity
            var timeEntryEntity = mapper.Map<Entities.TimeEntry>(timeEntry);
            timeEntryEntity.UserId = userId;

            await timeTrackerRepository.AddTimeEntryAsync(timeEntryEntity);
            await timeTrackerRepository.SaveChangesAsync();

            // return the created team
            var createdTimeEntryToReturn = mapper.Map<TimeEntryDto>(timeEntryEntity);
            return CreatedAtRoute("GetTimeEntry",
                new { userId = createdTimeEntryToReturn.UserID, id = createdTimeEntryToReturn.Id },
                createdTimeEntryToReturn);
        }

        // Perhaps change this controller to api/user/{userId}/timeentries
        // This would also mean I trim down the DTO for create so dont need to pass in userId
        /*

        [HttpPut("")]
        public async Task<ActionResult<TimeEntryDto>> UpdateTimeEntry([FromBody] TimeEntryForCreationDto timeEntry)
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
        */
    }
}