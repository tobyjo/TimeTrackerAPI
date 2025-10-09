using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.API.Entities
{
    public class TimeEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        // Foreign key for Project
        [Required]
        public int ProjectId { get; set; }

        // Navigation property
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        // Foreign key for SegmentType
        [Required]
        public int SegmentTypeId { get; set; }

        // Navigation property
        [ForeignKey("SegmentTypeId")]
        public SegmentType SegmentType { get; set; }

        // Foreign key for User
        [Required]
        public int UserId { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public User User { get; set; }

        public TimeEntry() { }

        public TimeEntry(DateTime startDateTime, DateTime endDateTime, int projectId, int segmentTypeId, int userId)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            ProjectId = projectId;
            SegmentTypeId = segmentTypeId;
            UserId = userId;
        }
    }
}
