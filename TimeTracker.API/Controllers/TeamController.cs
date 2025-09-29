using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Services;
using TimeTracker.API.Models;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/team")]
    public class TeamController : ControllerBase
    {
        private readonly ITimeTrackerRepository timeTrackerRepository;
        private readonly IMapper mapper;

        public TeamController(ITimeTrackerRepository timeTrackerRepository, IMapper mapper)
        {
            this.timeTrackerRepository = timeTrackerRepository ?? throw new ArgumentNullException(nameof(timeTrackerRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamWithoutUsersDto>>> GetTeams()
        {
            var teams = await timeTrackerRepository.GetTeamsAsync();
            var results = mapper.Map<IEnumerable<TeamWithoutUsersDto>>(teams);
            //var results = new List<TeamWithoutUsersDto>();
            //foreach (var team in teams)
            //{
            //    results.Add(new TeamWithoutUsersDto
            //    {
            //        TeamName = team.TeamName
            //    });
            //}
            return Ok(results);
        }

        // Return IActionResult as we dont know which DTO will be returned so go basic
        // - see "Demo: Returning Data from the Repository ... Part 2
        [HttpGet("{id}", Name ="GetTeam")]
        public async Task<IActionResult> GetTeam(int id, bool includeUsers = false)
        {
           var team = await timeTrackerRepository.GetTeamAsync(id, includeUsers);
            if (team == null)
                return NotFound();
            if (includeUsers)
            {
                var teamResult = mapper.Map<TeamWithUsersDto>(team);
                //var teamResult = new TeamWithUsersDto();
                //teamResult.TeamName = team.TeamName;
                //teamResult.Users = new List<TeamWithoutUsersDto>();
                //foreach( var user in team.Users )
                //{

                //}

                
                return Ok(teamResult);
            }
            else
            {
                var teamResult = mapper.Map<TeamWithoutUsersDto>(team);
                return Ok(teamResult);
            }
        }


        [HttpPost]
        public async Task<ActionResult<TeamWithoutUsersDto>> CreateTeam(TeamForCreationDto team)
        {
            // map to entity
            var teamEntity = mapper.Map<Entities.Team>(team);

            await timeTrackerRepository.AddTeamAsync(teamEntity);
            await timeTrackerRepository.SaveChangesAsync();

            // add to repository
            // no save as no changes to make in this demo
            // return the created team
            var createdTeamToReturn = mapper.Map<TeamWithoutUsersDto>(teamEntity);
            return CreatedAtRoute("GetTeam",
                new { id = createdTeamToReturn.Id },
                createdTeamToReturn);
        }

    }
}
