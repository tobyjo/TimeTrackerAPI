using Microsoft.EntityFrameworkCore;
using TimeTracker.API.Entities;

namespace TimeTracker.API.DbContexts
{
    public class TimeTrackerContext : DbContext
    {
        public DbSet<TimeEntry> TimeEntries { get; set; }

        public DbSet<SegmentType> SegmentTypes { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<User> Users { get; set; }
        public TimeTrackerContext(DbContextOptions<TimeTrackerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
      
            modelBuilder.Entity<TimeEntry>()
                .HasData(new TimeEntry(
                    startDateTime: DateTime.Parse("2025-08-01T09:00:00"),
                    endDateTime: DateTime.Parse("2025-08-01T17:00:00"),
                    projectId: 1,
                    segmentTypeId: 1,
                    userId: 1)
                {Id = 1},
                new TimeEntry(
                    startDateTime: DateTime.Parse("2025-08-02T09:00:00"), 
                    endDateTime: DateTime.Parse("2025-08-02T13:00:00"),
                    projectId: 1, 
                    segmentTypeId: 2,
                    userId: 1)
                {Id = 2},
                   new TimeEntry(
                       DateTime.Parse("2025-08-02T13:00:00"), 
                       DateTime.Parse("2025-08-02T15:00:00"),
                       projectId: 2,
                       segmentTypeId: 3,
                       userId: 1)
                   {Id = 3}
                );



            modelBuilder.Entity<Project>()
              .HasData(new Project("BPC.001")
              {
                  Id = 1,
                  Description = "Berkshire Primary Care 001"
              },
              new Project("BP")
              {
                  Id = 2,
                  Description = "ARRS"
              }
              );

            modelBuilder.Entity<SegmentType>()
             .HasData(new SegmentType("Meeting")
             {
                 Id = 1
             },
             new SegmentType("Calls")
             {
                 Id = 2
             },             
             new SegmentType("Planning")
             {
                 Id = 3
             }
             );

            modelBuilder.Entity<User>()
           .HasData(new User("kirstine", "Kirstine Hall")
           {
               Id = 1
           },
           new User("toby", "Toby Jones")
           {
               Id = 2
           }
           );

            base.OnModelCreating(modelBuilder);
        }
    }
}
