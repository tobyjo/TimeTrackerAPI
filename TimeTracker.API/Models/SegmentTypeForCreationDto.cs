using System.ComponentModel.DataAnnotations;

namespace TimeTracker.API.Models
{
    public class SegmentTypeForCreationDto
    {
        [Required(ErrorMessage = "You should provide a Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool IsVisible { get; set; } = true;

        [Required(ErrorMessage = "You should provide a TeamId")]
        public int TeamId { get; set; }
    }
}
