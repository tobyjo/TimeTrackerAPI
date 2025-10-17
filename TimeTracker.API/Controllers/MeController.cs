using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeTracker.API.Entities;
using TimeTracker.API.Extensions;
using TimeTracker.API.Models;
using TimeTracker.API.Services;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/me")]
    [Authorize]
    public class MeController : ControllerBase
    {
        private readonly ITimeTrackerRepository timeTrackerRepository;
        private readonly IMapper mapper;

        public MeController(ITimeTrackerRepository timeTrackerRepository, IMapper mapper)
        {
            this.timeTrackerRepository = timeTrackerRepository ?? throw new ArgumentNullException(nameof(timeTrackerRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private string GetCurrentUserId() =>
        User.GetAuth0UserId() ?? throw new InvalidOperationException("Authenticated user id not found in token.");

        [HttpGet("timeentries", Name = "GetMeWithTimeEntries")]
        public async Task<IActionResult> GetMeWithTimeEntries(
          [FromQuery] DateTime? startDateTime,
          [FromQuery] DateTime? endDateTime
          )
        {
            var userId = GetCurrentUserId();

            // If both dates are provided, filter by range
            if (startDateTime.HasValue && endDateTime.HasValue)
            {
                var entries = await timeTrackerRepository.GetUserWithTimeEntriesWithDateRangeAsync(
                    userId, startDateTime.Value, endDateTime.Value);
                if (entries == null)
                {
                    return NotFound();
                }
                var userResultWithDateTime = mapper.Map<UserWithTimeEntriesDto>(entries);
                return Ok(userResultWithDateTime);
            }
            // Otherwise return all time entries for user
            var user = await timeTrackerRepository.GetUserWithTimeEntriesAsync(userId);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserWithTimeEntriesDto>(user);
            return Ok(userResult);
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetMyProjects()
        {
            var userId = GetCurrentUserId();

            var user = await timeTrackerRepository.GetUserWithProjectsAsync(userId);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserWithProjectsDto>(user);
            return Ok(userResult);
        }

        [HttpGet("segmenttypes", Name = "GetMySegmentTypes")]
        public async Task<IActionResult> GetUserWithSegments()
        {
            var userId = GetCurrentUserId();

            var user = await timeTrackerRepository.GetUserWithSegmentTypesAsync(userId);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserWithSegmentTypesDto>(user);
            return Ok(userResult);

        }

        [HttpGet("timeentries/{id}", Name = "GetTimeEntry")]
        public async Task<IActionResult> GetTimeEntry(int id)
        {
            var userId = GetCurrentUserId(); 

            var timeEntry = await timeTrackerRepository.GetTimeEntryAsync(id);
            // Additional check to see if matches UserId
            if (timeEntry == null || !string.Equals(timeEntry.UserId, userId, StringComparison.OrdinalIgnoreCase))
                return NotFound();

            var timeEntryResult = mapper.Map<TimeEntryWithDetailsDto>(timeEntry);
            return Ok(timeEntryResult);

        }

        [HttpPost("timeentries")]
        public async Task<ActionResult<TimeEntryDto>> CreateTimeEntry( [FromBody] TimeEntryForCreationDto timeEntry)
        {
            var userId = GetCurrentUserId();

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
                new { id = createdTimeEntryToReturn.Id },
                createdTimeEntryToReturn);
        }


        [HttpPut("timeentries/{timeentryid}")]
        public async Task<ActionResult> UpdateTimeEntry(int timeEntryid, [FromBody] TimeEntryForUpdateDto timeEntry)
        {
            var userId = GetCurrentUserId();

 
            if (!await timeTrackerRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var timeEntryEntity = await timeTrackerRepository.GetTimeEntryAsync(timeEntryid);
            // Additional check to see if matches UserId
            if (timeEntryEntity == null || !string.Equals(timeEntryEntity.UserId, userId, StringComparison.OrdinalIgnoreCase))
                return NotFound();

            // Overwrite properties from db with those from incoming object
            mapper.Map(timeEntry, timeEntryEntity);
            timeEntryEntity.UserId = userId;
            await timeTrackerRepository.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("timeentries/{timeentryid}")]
        public async Task<ActionResult> DeleteTimeEntry(int timeEntryid)
        {
            var userId = GetCurrentUserId();

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
