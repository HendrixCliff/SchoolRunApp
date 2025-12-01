namespace SchoolRunApp.API.DTOs.Student
{
    public class ActivateStudentDto
    {
        public string AdmissionNumber { get; set; } = string.Empty;
        public string ActivationCode { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
