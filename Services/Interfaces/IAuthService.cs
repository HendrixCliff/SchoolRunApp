using SchoolRunApp.API.DTOs.Auth;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<bool> RequestPasswordResetAsync(RequestPasswordResetDto dto);
        Task<bool> VerifyResetCodeAsync(VerifyResetCodeDto dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshRequestDto dto);
        Task<bool> RevokeRefreshTokenAsync(RevokeRefreshDto dto);
    }
}
