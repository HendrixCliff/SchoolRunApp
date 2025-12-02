namespace SchoolRunApp.API.DTOs.Auth
{
    public class RevokeRefreshDto
    {
        public string Email { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
