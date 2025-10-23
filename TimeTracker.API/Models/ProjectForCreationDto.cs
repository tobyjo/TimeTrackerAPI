using System.ComponentModel.DataAnnotations;

namespace TimeTracker.API.Models
{
    public class ProjectForCreationDto
    {
        [Required(ErrorMessage = "You should provide a Project Code")]
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public bool IsVisible { get; set; } = true;

        [Required(ErrorMessage = "You should provide a TeamId")]
        public int TeamId { get; set; }
    }
}
