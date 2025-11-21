namespace SchoolRunApp.API.DTOs.Auth
{
    public class RegisterDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

      
        public string Role { get; set; } = "Student";

        public string? AdmissionNumber { get; set; }
        public string? TeacherId { get; set; }

        
        public string? DateOfBirth { get; set; } // optional, can be used for extra check
    }
}
