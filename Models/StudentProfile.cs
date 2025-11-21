using System;

namespace SchoolRunApp.API.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }

        public string AdmissionNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; } = "N/A";

        
        public int ClassId { get; set; }
        public Class? Class { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
       public ICollection<Result> Results { get; set; } = new List<Result>();
        public ICollection<StudentActivity> Activities { get; set; } = new List<StudentActivity>();

    }
}
