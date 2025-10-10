namespace TimeTracker.API.Models
{
    public class TimeEntryWithDetailsDto
    {
        public int Id { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string ProjectCode { get; set; } = string.Empty;

        public int ProjectID { get; set; }
        public int UserID { get; set; }
        public string ProjectDescription { get; set; } = string.Empty;

        public string SegmentTypeName { get; set; } = string.Empty;

        public int SegmentTypeID { get; set; }
    }
}