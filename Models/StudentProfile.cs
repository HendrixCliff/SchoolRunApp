using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }

        public string AdmissionNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public User User { get; set; }
        public Class Class { get; set; }
    }
}
