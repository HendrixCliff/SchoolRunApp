using System;
using SchoolRunApp.API.Models;


namespace SchoolRunApp.API.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public DateTime DatePosted { get; set; } = DateTime.UtcNow;

        // User who posted it
        public int PostedByUserId { get; set; }
        public User? PostedByUser { get; set; }

        // Type: General or Targeted
        public AnnouncementType Type { get; set; }

        // Optional target groups for teachers
        public int? ClassId { get; set; }
        public Class? Class { get; set; }

        public int? SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public int? StudentId { get; set; }
        public StudentProfile? Student { get; set; }
    }

    public enum AnnouncementType
    {
        General = 1,
        Targeted = 2
    }

}
