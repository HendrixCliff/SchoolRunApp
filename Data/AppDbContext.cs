using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<StudentActivity> StudentActivities { get; set; }
        public DbSet<TeacherProfile> TeacherProfiles { get; set; }
         public DbSet<User> User { get; set; }
          public DbSet<EmailSettings> EmailSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One Class -> Many Students
            modelBuilder.Entity<StudentProfile>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassId);

            // One-to-One: User ↔ StudentProfile
            modelBuilder.Entity<StudentProfile>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<StudentProfile>(s => s.UserId);

            // One Subject -> Many Results
            modelBuilder.Entity<Result>()
                .HasOne(r => r.Subject)
                .WithMany(s => s.Results)
                .HasForeignKey(r => r.SubjectId);

            // One Student → Many Results
            modelBuilder.Entity<Result>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Results)
                .HasForeignKey(r => r.StudentId);
                
            modelBuilder.Entity<Result>()
                .HasOne(r => r.Class)
                .WithMany()
                .HasForeignKey(r => r.ClassId);

                  
                   //Many-to-many between Student and Activity
                modelBuilder.Entity<StudentActivity>()
                    .HasKey(sa => new { sa.StudentId, sa.ActivityId });

                modelBuilder.Entity<StudentActivity>()
                    .HasOne(sa => sa.Student)
                    .WithMany(s => s.Activities)
                    .HasForeignKey(sa => sa.StudentId);

            modelBuilder.Entity<StudentActivity>()
                .HasOne(sa => sa.Activity)
                .WithMany(a => a.StudentActivities)
                .HasForeignKey(sa => sa.ActivityId);
                    
                            modelBuilder.Entity<Announcement>()
            .HasOne(a => a.Class)
            .WithMany()
            .HasForeignKey(a => a.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.Subject)
            .WithMany()
            .HasForeignKey(a => a.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

       }
    }
}
