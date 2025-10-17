using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.API.Entities
{
    public class User
    {
        [Key]
        [MaxLength(255)]
        public string Id { get; set; }


        [MaxLength(20)]
        public string? UserName { get; set; }


        [MaxLength(50)]
        public string? FullName { get; set; }

        [ForeignKey("TeamId")]
        public int TeamId { get; set; }

        public Team Team { get; set; }

        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();

        /*
        public User(string userName, string fullName)
        {
            UserName = userName;
            FullName = fullName;
        }
        */

        public User(string Id) 
        {
            this.Id = Id;
        }

        // Parameterless constructor for EF
        public User()
        {
         
        }
    }
}
