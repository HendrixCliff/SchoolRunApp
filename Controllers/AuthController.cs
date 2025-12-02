using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolRunApp.API.DTOs.Auth;
using SchoolRunApp.API.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var r = await _authService.LoginAsync(dto);
                return Ok(r);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Invalid login attempt for {Email}", dto?.Email);
                return Unauthorized(new { message = "Invalid credentials" });
            }
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetDto dto)
        {
            var ok = await _authService.RequestPasswordResetAsync(dto);
            return Ok(new { success = ok, message = "If your email exists, a reset code has been sent." });
        }

        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyReset(VerifyResetCodeDto dto)
        {
            var ok = await _authService.VerifyResetCodeAsync(dto);
            return Ok(new { valid = ok });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var ok = await _authService.ResetPasswordAsync(dto);
            if (!ok) return BadRequest("Invalid or expired reset code.");

            return Ok(new { message = "Password reset successfully." });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            try
            {
                var r = await _authService.RefreshTokenAsync(dto);
                return Ok(r);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Invalid refresh attempt for {Email}", dto?.Email);
                return BadRequest(new { message = "Invalid refresh token" });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RevokeRefreshDto dto)
        {
            var ok = await _authService.RevokeRefreshTokenAsync(dto);
            return Ok(new { success = ok });
        }
    }
}
