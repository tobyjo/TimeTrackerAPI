using TimeTracker.API.Entities;

namespace TimeTracker.API.Models
{
    public class TeamWithProjectsDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    }
}
