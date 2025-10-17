using System.ComponentModel.DataAnnotations;

namespace TimeTracker.API.Models
{
    public class UserForCreationDto
    {
        [Required(ErrorMessage = "You should provide a User Id")]
        public string UserId { get; set; }
    }
}
