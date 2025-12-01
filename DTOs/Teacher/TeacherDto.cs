
    namespace SchoolRunApp.API.DTOs.Teacher
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string TeacherCode { get; set; } = "";
        public string Qualification { get; set; } = "";
        public string? Department { get; set; }
        public DateTime DateJoined { get; set; }
    }
}


