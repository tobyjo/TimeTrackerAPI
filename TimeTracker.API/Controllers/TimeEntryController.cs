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
            if (!await timeTrackerRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

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



        [HttpPut("{timeentryid}")]
        public async Task<ActionResult> UpdateTimeEntry(int userId, int timeEntryid, [FromBody] TimeEntryForUpdateDto timeEntry)
        {
            if(! await timeTrackerRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var timeEntryEntity = await timeTrackerRepository.GetTimeEntryAsync(timeEntryid);
            if( timeEntryEntity == null )
            {
                return NotFound();
            }

            // Overwrite properties from db with those from incoming object
            mapper.Map(timeEntry, timeEntryEntity);
            await timeTrackerRepository.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{timeentryid}")]
        public async Task<ActionResult> DeleteTimeEntry(int userId, int timeEntryid)
        {
            if (!await timeTrackerRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            var timeEntryEntity = await timeTrackerRepository.GetTimeEntryAsync(timeEntryid);
            if (timeEntryEntity == null)
            {
                return NotFound();
            }
    
            // At this point context has timeentry in memory so no need for an async call here
            timeTrackerRepository.DeleteTimeEntry(timeEntryEntity);

            // And async required to write to db
            await timeTrackerRepository.SaveChangesAsync();
            return NoContent();
        }

    }
}