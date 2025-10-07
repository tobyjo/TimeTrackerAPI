using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.API.Entities
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string TeamName { get; set; }

        // One-to-many: Team has many Users
        public ICollection<User> Users { get; set; } = new List<User>();

        // One-to-many: Team has many Projects
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        // One-to-many: Team has many SegmentTypes
        public ICollection<SegmentType> SegmentTypes { get; set; } = new List<SegmentType>();

        public Team( string teamName)
        {
            TeamName = teamName;
        }
    }
}
