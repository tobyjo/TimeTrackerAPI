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

        // Not required as will be found by EF because of ICollection in Team but adds clarity
        // If we use convention we also dont need to label it as foreign key but again, it adds clarity
        [ForeignKey("TeamId")]
        public int TeamId { get; set; }

        // Need Team attribute to point back to parent Team - required for stopping cascading delete in OnModelBuilder
        public Team Team { get; set; }

        // One-to-many: User has many TimeEntries
        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();

        public User(string userName, string fullName)
        {
            UserName = userName;
            FullName = fullName;
        }
    }
}
