using TimeTracker.API.Entities;

namespace TimeTracker.API.Models
{
    public class TeamWithUsersDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }

        public ICollection<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
