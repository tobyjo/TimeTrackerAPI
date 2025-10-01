using System.ComponentModel.DataAnnotations;

namespace TimeTracker.API.Models
{
    public class TimeEntryForCreationDto
    {
        [Required(ErrorMessage = "You should provide a Start Date and Time")]
        public DateTime StartDateTime { get; set; }


        [Required(ErrorMessage = "You should provide an End Date and Time")]
        public DateTime EndDateTime { get; set; }


        [Required(ErrorMessage = "You should provide a Project Id")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "You should provide a SegmentType Id")]
        public int SegmentTypeId { get; set; }


        [Required(ErrorMessage = "You should provide a UserId")]
        public int UserId { get; set; }
    }
}
