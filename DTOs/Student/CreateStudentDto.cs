namespace SchoolRunApp.API.DTOs.Student
{
    public class CreateStudentDto
    {
        public string FullName { get; set; } = string.Empty;
        public string AdmissionNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public int ClassId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
