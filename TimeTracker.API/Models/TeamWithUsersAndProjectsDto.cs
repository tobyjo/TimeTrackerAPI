using TimeTracker.API.Entities;

namespace TimeTracker.API.Models
{
    public class TeamWithUsersAndProjectsDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public ICollection<UserDto> Users { get; set; } = new List<UserDto>();
        public ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    }
}
