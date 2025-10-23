using System.ComponentModel.DataAnnotations;

namespace TimeTracker.API.Models
{
    public class TimeEntryForUpdateDto
    {
        [Required(ErrorMessage = "You should provide a Start Date and Time")]
        public DateTime StartDateTime { get; set; }


        [Required(ErrorMessage = "You should provide an End Date and Time")]
        public DateTime EndDateTime { get; set; }

        [MaxLength(300, ErrorMessage = "Note cannot exceed 300 characters")]
        public string Note { get; set; } = string.Empty;


        [Required(ErrorMessage = "You should provide a ProjectId")]
        [Range(1, int.MaxValue, ErrorMessage = "ProjectId must be greater than 0")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "You should provide a SegmentType Id")]
        [Range(1, int.MaxValue, ErrorMessage = "SegmentTypeId must be greater than 0")]
        public int SegmentTypeId { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDateTime <= StartDateTime)
            {
                yield return new ValidationResult(
                    "EndDateTime must be after StartDateTime.",
                    new[] { nameof(EndDateTime) });
            }
        }
    }
}
