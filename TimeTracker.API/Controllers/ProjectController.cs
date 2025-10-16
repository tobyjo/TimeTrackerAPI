using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Entities;
using TimeTracker.API.Models;
using TimeTracker.API.Services;

namespace TimeTracker.API.Controllers
{

    [ApiController]
    [Route("api/project")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly ITimeTrackerRepository timeTrackerRepository;
        private readonly IMapper mapper;

        public ProjectController(ITimeTrackerRepository timeTrackerRepository, IMapper mapper)
        {
            this.timeTrackerRepository = timeTrackerRepository ?? throw new ArgumentNullException(nameof(timeTrackerRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /*

        [HttpGet]
        public ActionResult<IEnumerable<ProjectDto>> Get([FromQuery] int? projectId)
        {
            var projects = _store.Data.Project;
            if (projectId.HasValue)
                projects = projects.Where(e => e.Id == projectId.Value).ToList();



            

            return Ok(projects);
        }
        */

        // TODO - Get project for AddProject can refer to it in response

        [HttpGet("{id}", Name = "GetProject")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await timeTrackerRepository.GetProjectAsync(id);

            if (project == null)
                return NotFound();

            var projectResult = mapper.Map<ProjectDto>(project);
            return Ok(projectResult);
        }

       [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject(ProjectForCreationDto project)
        {
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





        /*

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
        */
    }
}
