namespace TimeTracker.API.Models
{
    public class SegmentTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool IsVisible { get; set; } = true;

        public int TeamId { get; set; }
    }
}