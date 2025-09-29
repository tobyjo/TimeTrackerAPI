using System.ComponentModel.DataAnnotations;

namespace TimeTracker.API.Models
{
    public class TeamForCreationDto
    {
        [Required(ErrorMessage = "You should provide a TeamName")]
        [MaxLength(100)]
        public string TeamName { get; set; }
    }
}
