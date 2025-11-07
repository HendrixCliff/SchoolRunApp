using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public double Score { get; set; }
        public string Term { get; set; } = string.Empty;
        public string Session { get; set; } = string.Empty;

        public StudentProfile Student { get; set; }
        public Subject Subject { get; set; }
    }
}
