using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Models;
using TimeTracker.API.Services;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/segmenttype")]
    public class SegmentTypeController : ControllerBase
    {
        private readonly ITimeTrackerRepository timeTrackerRepository;
        private readonly IMapper mapper;

        public SegmentTypeController(ITimeTrackerRepository timeTrackerRepository, IMapper mapper)
        {
            this.timeTrackerRepository = timeTrackerRepository ?? throw new ArgumentNullException(nameof(timeTrackerRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}", Name = "GetSegmentType")]
        public async Task<IActionResult> GetProject(int id)
        {
            var segmentType = await timeTrackerRepository.GetSegmentTypeAsync(id);

            if (segmentType == null)
                return NotFound();

            var segmentTypeResult = mapper.Map<SegmentTypeDto>(segmentType);
            return Ok(segmentTypeResult);
        }

    }
}
