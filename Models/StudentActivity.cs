using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Models
{
    public class StudentActivity
    {
        public int StudentId { get; set; }
        public StudentProfile Student { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
