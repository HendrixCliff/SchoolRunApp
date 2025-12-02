namespace SchoolRunApp.API.DTOs.Auth
{
    public class RefreshRequestDto
    {
        public string Email { get; set; } = string.Empty;        
        public string RefreshToken { get; set; } = string.Empty;
    }
}
