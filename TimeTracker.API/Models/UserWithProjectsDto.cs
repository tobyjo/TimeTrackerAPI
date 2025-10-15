namespace TimeTracker.API.Models
{
    public class UserWithProjectsDto
    {
        public string Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        public ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    }
}
