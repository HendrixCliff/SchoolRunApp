namespace SchoolRunApp.API.DTOs.Teacher
{
    public class ActivateTeacherDto
    {
        public string TeacherCode { get; set; } = string.Empty;
        public string ActivationCode { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
