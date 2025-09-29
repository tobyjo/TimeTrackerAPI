namespace TimeTracker.API.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
