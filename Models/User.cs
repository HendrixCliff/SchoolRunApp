namespace SchoolRunApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = "Admin"; // Student/Admin/Teacher
        public string? PasswordHash { get; set; }
        public string? PasswordResetCode { get; set; }
        public DateTime? PasswordResetExpiry { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public bool IsActive { get; set; } = false;
        public string? TempActivationCode { get; set; }
    }
}
