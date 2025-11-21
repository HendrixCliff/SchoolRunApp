using System;

namespace SchoolRunApp.API.Models
{
    public class TeacherProfile
    {
        public int Id { get; set; }
        public string TeacherCode { get; set; } = string.Empty; 
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
