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

        public DbSet<Team> Teams { get; set; }

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
                    userId: "auth0|user1")
                {Id = 1},
                new TimeEntry(
                    startDateTime: DateTime.Parse("2025-08-02T09:00:00"), 
                    endDateTime: DateTime.Parse("2025-08-02T13:00:00"),
                    projectId: 1, 
                    segmentTypeId: 2,
                    userId: "auth0|user1")
                {Id = 2},
                   new TimeEntry(
                       DateTime.Parse("2025-08-02T13:00:00"), 
                       DateTime.Parse("2025-08-02T15:00:00"),
                       projectId: 2,
                       segmentTypeId: 3,
                       userId: "auth0|user1")
                   {Id = 3}
                );

            modelBuilder.Entity<Project>()
              .HasData(new Project("BPC")
              {
                  Id = 1,
                  Description = "Berkshire Primary Care",
                  TeamId = 1
              },
              new Project("Mag House")
              {
                  Id = 2,
                  Description = "Mag House",
                  TeamId = 1
              }
              );

            modelBuilder.Entity<Team>()
             .HasData(new Team("BPC")
             {
                 Id = 1
             }
             );

            modelBuilder.Entity<SegmentType>()
             .HasData(new SegmentType("Board")
             {
                 Id = 1,
                 TeamId = 1
             },
             new SegmentType("Strategy")
             {
                 Id = 2,
                 TeamId = 1
             },             
             new SegmentType("Recall")
             {
                 Id = 3,
                 TeamId = 1
             }
             );

            modelBuilder.Entity<User>()
           .HasData(new User("kirstine", "Kirstine Hall")
           {
               Id = "auth0|user1",
               TeamId = 1
           },
           new User("toby", "Toby Jones")
           {
               Id = "auth0|68efba26b0f9e98d45be40b4",
               TeamId = 1
           }
           );


            // Stop warning about possible cycles or multiple cascade paths
            // When deleting a Team it wont automatically delete the associated Projects and Users
            // Restrict delete for Team -> Projects
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Projects)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Restrict delete for Team -> Users
            modelBuilder.Entity<User>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
