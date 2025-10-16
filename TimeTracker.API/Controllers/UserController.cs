using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Models;
using TimeTracker.API.Services;
using System.Security.Claims;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ITimeTrackerRepository timeTrackerRepository;
        private readonly IMapper mapper;

        public UserController(ITimeTrackerRepository timeTrackerRepository, IMapper mapper)
        {
            this.timeTrackerRepository = timeTrackerRepository ?? throw new ArgumentNullException(nameof(timeTrackerRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Helper to check if the route userId matches the authenticated user
        private bool IsUserAuthorized(string userId)
        {
            var authenticatedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.Equals(userId, authenticatedUserId, StringComparison.OrdinalIgnoreCase);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string id)
        {
            if (!IsUserAuthorized(id))
                return Forbid();

            var user = await timeTrackerRepository.GetUserAsync(id);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserDto>(user);
            return Ok(userResult);

        }

        [HttpGet("{id}/timeentries", Name = "GetUserWithTimeEntries")]
        public async Task<IActionResult> GetUserWithTimeEntries(
            string id,
            [FromQuery] DateTime? startDateTime,
            [FromQuery] DateTime? endDateTime
            )
        {
            if (!IsUserAuthorized(id))
                return Forbid();

            // If both dates are provided, filter by range
            if (startDateTime.HasValue && endDateTime.HasValue)
            {
                var entries = await timeTrackerRepository.GetUserWithTimeEntriesWithDateRangeAsync(
                    id, startDateTime.Value, endDateTime.Value);
                if (entries == null)
                {
                    return NotFound();
                }
                var userResultWithDateTime = mapper.Map<UserWithTimeEntriesDto>(entries);
                return Ok(userResultWithDateTime);
            }
            // Otherwise return all time entries for user
            var user = await timeTrackerRepository.GetUserWithTimeEntriesAsync(id);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserWithTimeEntriesDto>(user);
            return Ok(userResult);
        }

        [HttpGet("{id}/projects", Name = "GetUserWithProjects")]
        public async Task<IActionResult> GetUserWithProjects(string id)
        {
            if (!IsUserAuthorized(id))
                return Forbid();

            var user = await timeTrackerRepository.GetUserWithProjectsAsync(id);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserWithProjectsDto>(user);
            return Ok(userResult);

        }

        [HttpGet("{id}/segmenttypes", Name = "GetUserWithSegmentTypes")]
        public async Task<IActionResult> GetUserWithSegments(string id)
        {
            if (!IsUserAuthorized(id))
                return Forbid();

            var user = await timeTrackerRepository.GetUserWithSegmentTypesAsync(id);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserWithSegmentTypesDto>(user);
            return Ok(userResult);

        }

    }

  }
