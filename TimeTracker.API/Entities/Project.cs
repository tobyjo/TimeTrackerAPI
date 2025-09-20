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

        // One-to-many: Project has many TimeEntries
        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();


        public Project( string code)
        {
            Code = code;
        }
    }
}
