using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.API.Entities
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        // Not required as will be found by EF because of ICollection in Team but adds clarity
        // If we use convention we also dont need to label it as foreign key but again, it adds clarity
        [ForeignKey("TeamId")]
        public int TeamId { get; set; }

        // Need Team attribute to point back to parent Team - required for stopping cascading delete in OnModelBuilder
        public Team Team { get; set; }


        // Not required as will be found by EF because of ICollection in Team but adds clarity
        // If we use convention we also dont need to label it as foreign key but again, it adds clarity
        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();


        public Project( string code)
        {
            Code = code;
        }
    }
}
