public class RootDto
{
    public List<ProjectDto> Project { get; set; } = new();
    public List<SegmentTypeDto> SegmentType { get; set; } = new();
    public List<TimeEntryDto> TimeEntry { get; set; } = new();
}