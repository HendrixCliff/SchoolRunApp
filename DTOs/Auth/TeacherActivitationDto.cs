namespace SchoolRunApp.API.DTOs.Auth
{
    public class TeacherActivationDto
    {
        public string Email { get; set; } = string.Empty;
        public string ActivationCode { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
