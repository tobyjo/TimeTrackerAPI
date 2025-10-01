namespace TimeTracker.API.Models
{
    public class UserWithTimeEntriesDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        // public ICollection<TimeEntryDto> TimeEntries { get; set; } = new List<TimeEntryDto>();
        public ICollection<TimeEntryWithDetailsDto> TimeEntries { get; set; } = new List<TimeEntryWithDetailsDto>();
    }
}
