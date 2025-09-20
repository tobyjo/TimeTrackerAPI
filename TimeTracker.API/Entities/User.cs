using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.API.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        // One-to-many: Project has many TimeEntries
        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();

        public User(string userName, string fullName)
        {
            UserName = userName;
            FullName = fullName;
        }
    }
}
