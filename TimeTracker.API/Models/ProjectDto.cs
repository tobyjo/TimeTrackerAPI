namespace TimeTracker.API.Models
{
    public class ProjectDto
    {

        public string Code { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}