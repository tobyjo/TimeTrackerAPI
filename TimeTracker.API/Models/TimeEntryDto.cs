public class TimeEntryDto
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }

    public int ProjectID { get; set; }
    public int SegmentTypeID { get; set; }

    public int UserID { get; set; }
}