namespace TimeTracker.API.Models
{
    public class UserWithSegmentTypesDto
    {

            public int Id { get; set; }
            public string UserName { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;

            public ICollection<SegmentTypeDto> SegmentTypes { get; set; } = new List<SegmentTypeDto>();
        }

}
