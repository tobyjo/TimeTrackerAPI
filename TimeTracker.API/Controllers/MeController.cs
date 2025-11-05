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

        [HttpGet("projects/{id}", Name = "GetProject")]
        public async Task<IActionResult> GetProject(int id)
        {
            var userId = GetCurrentUserId();

            var project = await timeTrackerRepository.GetProjectAsync(id);

            if (project == null)
                return NotFound();

            // Check that the Team Id matches the users Team ID and of course, that the user exists
            var user = await timeTrackerRepository.GetUserAsync(userId);
            if (user == null || user.TeamId != project.TeamId)
            {
                return BadRequest("Invalid TeamId for the current user.");
            }

                var projectResult = mapper.Map<ProjectDto>(project);
            return Ok(projectResult);
        }

        // tri-state query param: ?isVisible=true|false  (omit for all)
        [HttpGet("projects")]
        public async Task<IActionResult> GetMyProjects([FromQuery] bool? isVisible)
        {
            var userId = GetCurrentUserId();

            var user = await timeTrackerRepository.GetUserWithProjectsAsync(userId, isVisible);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserWithProjectsDto>(user);
            return Ok(userResult);
        }

        [HttpPost("projects")]
        public async Task<ActionResult<ProjectDto>> CreateProject(ProjectForCreationDto project)
        {
            var userId = GetCurrentUserId();

            /*
            if (!await timeTrackerRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }
            */

            // Check that the Team Id matches the users Team ID and of course, that the user exists
            var user = await timeTrackerRepository.GetUserAsync(userId);
            if( user == null || user.TeamId != project.TeamId)
            {
                return BadRequest("Invalid TeamId for the current user.");
            }

            // map to entity
            var projectEntity = mapper.Map<Entities.Project>(project);

            await timeTrackerRepository.AddProjectAsync(projectEntity);
            await timeTrackerRepository.SaveChangesAsync();

            // return the created team
            var createdProjectToReturn = mapper.Map<ProjectDto>(projectEntity);
            return CreatedAtRoute("GetProject",
                new { id = createdProjectToReturn.Id },
                createdProjectToReturn);
        }


        [HttpPut("projects/{projectid}")]
        public async Task<ActionResult> UpdateProject(int projectId, [FromBody] ProjectForUpdateDto project)
        {
            var userId = GetCurrentUserId();

            // Check that the Team Id matches the users Team ID and of course, that the user exists
            var user = await timeTrackerRepository.GetUserAsync(userId);
            if (user == null || user.TeamId != project.TeamId)
            {
                return BadRequest("Invalid TeamId for the current user.");
            }

            var projectEntity = await timeTrackerRepository.GetProjectAsync(projectId);
            if ( projectEntity is null)
            {
                return NotFound();
            }

            // Overwrite properties from db with those from incoming object
            mapper.Map(project, projectEntity);
            await timeTrackerRepository.SaveChangesAsync();

            return NoContent();

        }

        [HttpGet("segmenttypes/{id}", Name = "GetSegmentType")]
        public async Task<IActionResult> GetSegmentType(int id)
        {
            var userId = GetCurrentUserId();

            var segmentType = await timeTrackerRepository.GetSegmentTypeAsync(id);

            if (segmentType == null)
                return NotFound();

            // Check that the Team Id matches the users Team ID and of course, that the user exists
            var user = await timeTrackerRepository.GetUserAsync(userId);
            if (user == null || user.TeamId != segmentType.TeamId)
            {
                return BadRequest("Invalid TeamId for the current user.");
            }

            var segmentTypeResult = mapper.Map<SegmentTypeDto>(segmentType);
            return Ok(segmentTypeResult);
        }

        // tri-state query param: ?isVisible=true|false  (omit for all)
        [HttpGet("segmenttypes", Name = "GetMySegmentTypes")]
        public async Task<IActionResult> GetMySegmentTypes([FromQuery] bool? isVisible)
        {
            var userId = GetCurrentUserId();

            var user = await timeTrackerRepository.GetUserWithSegmentTypesAsync(userId, isVisible);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserWithSegmentTypesDto>(user);
            return Ok(userResult);

        }

        [HttpPost("segmenttypes")]
        public async Task<ActionResult<ProjectDto>> CreateSegmentType(SegmentTypeForCreationDto segmentType)
        {
            var userId = GetCurrentUserId();

            // Check that the Team Id matches the users Team ID and of course, that the user exists
            var user = await timeTrackerRepository.GetUserAsync(userId);
            if (user == null || user.TeamId != segmentType.TeamId)
            {
                return BadRequest("Invalid TeamId for the current user.");
            }

            // map to entity
            var segmentTypeEntity = mapper.Map<Entities.SegmentType>(segmentType);

            await timeTrackerRepository.AddSegmentTypeAsync(segmentTypeEntity);
            await timeTrackerRepository.SaveChangesAsync();

            // return the created team
            var createdSegmentTypeToReturn = mapper.Map<SegmentTypeDto>(segmentTypeEntity);
            return CreatedAtRoute("GetSegmentType",
                new { id = createdSegmentTypeToReturn.Id },
                createdSegmentTypeToReturn);
        }

        [HttpPut("segmenttypes/{segmenttypeid}")]
        public async Task<ActionResult> UpdateSegmentType(int segmentTypeId, [FromBody] SegmentTypeForUpdateDto segmentType)
        {
            var userId = GetCurrentUserId();

            // Check that the Team Id matches the users Team ID and of course, that the user exists
            var user = await timeTrackerRepository.GetUserAsync(userId);
            if (user == null || user.TeamId != segmentType.TeamId)
            {
                return BadRequest("Invalid TeamId for the current user.");
            }

            var segmentTypeEntity = await timeTrackerRepository.GetSegmentTypeAsync(segmentTypeId);
            if (segmentTypeEntity is null)
            {
                return NotFound();
            }

            // Overwrite properties from db with those from incoming object
            mapper.Map(segmentType, segmentTypeEntity);
            await timeTrackerRepository.SaveChangesAsync();

            return NoContent();

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
