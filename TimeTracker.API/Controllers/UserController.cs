using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Models;
using TimeTracker.API.Services;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ITimeTrackerRepository timeTrackerRepository;
        private readonly IMapper mapper;

        public UserController(ITimeTrackerRepository timeTrackerRepository, IMapper mapper)
        {
            this.timeTrackerRepository = timeTrackerRepository ?? throw new ArgumentNullException(nameof(timeTrackerRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await timeTrackerRepository.GetUserAsync(id);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserDto>(user);
            return Ok(userResult);

        }

     
        [HttpGet("{id}/timeentries", Name = "GetUserWithTimeEntries")]
        public async Task<IActionResult> GetUserWithTimeEntries(int id)
        {
            var user = await timeTrackerRepository.GetUserWithTimeEntriesAsync(id);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserWithTimeEntriesDto>(user);
            return Ok(userResult);

        }

        [HttpGet("{id}/projects", Name = "GetUserWithProjects")]
        public async Task<IActionResult> GetUserWithProjects(int id)
        {
            var user = await timeTrackerRepository.GetUserWithProjectsAsync(id);
            if (user == null)
                return NotFound();

            var userResult = mapper.Map<UserWithProjectsDto>(user);
            return Ok(userResult);

        }

    }

  }
