namespace SchoolRunApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = "Teacher"; // Student/Admin/Teacher
        public string? PasswordHash { get; set; }

        public bool IsActive { get; set; } = false;
        public string? TempActivationCode { get; set; }
    }
}
