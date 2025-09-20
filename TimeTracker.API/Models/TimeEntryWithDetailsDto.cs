namespace TimeTracker.API.Models
{
    public class TimeEntryWithDetailsDto
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string ProjectCode { get; set; } = string.Empty;
        public int UserID { get; set; }
        public string ProjectDescription { get; set; } = string.Empty;

        public string SegmentTypeName { get; set; } = string.Empty;
    }
}