namespace SchoolRunApp.API.DTOs.Student
{
    public class UpdateStudentDto
    {
        public string AdmissionNumber { get; set; } = "";
        public string Gender { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public int ClassId { get; set; }
    }
}
