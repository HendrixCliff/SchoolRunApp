using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

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
        }
    }
}
