using System;

namespace SchoolRunApp.API.Models
{
    public class TeacherProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TeacherCode { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string? Department { get; set; }
        public DateTime DateJoined { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
    }
}
