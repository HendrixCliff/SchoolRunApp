namespace SchoolRunApp.API.DTOs.Teacher
{
    public class CreateTeacherDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }

        public string TeacherCode { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string? Department { get; set; }
        public DateTime? DateJoined { get; set; }
    }
}
