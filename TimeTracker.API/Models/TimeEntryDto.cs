namespace TimeTracker.API.Models
{
    public class TimeEntryDto
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int ProjectID { get; set; }
        public int SegmentTypeID { get; set; }

        public int UserID { get; set; }
    }
}