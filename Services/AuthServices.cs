using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SchoolRunApp.API.DTOs.Auth;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.Services.Interfaces;

namespace SchoolRunApp.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
        private readonly IEmailService _emailService;
        private readonly int _refreshTokenMinutes;
        public AuthService(
            IUserRepository userRepo,
            IConfiguration config,
            ILogger<AuthService> logger,
            IEmailService emailService)
        {
            _userRepo = userRepo;
            _config = config;
            _logger = logger;
            _emailService = emailService;
             _refreshTokenMinutes = int.TryParse(_config["Jwt:RefreshExpiryMinutes"], out var m) ? m : 60 * 24 * 7; 
        }

          public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var user = await _userRepo.GetByEmailAsync(dto.Email.Trim());
            if (user == null)
                throw new InvalidOperationException("Invalid credentials.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid credentials.");

            if (!user.IsActive)
                throw new InvalidOperationException("Account not activated. Please activate your account.");

            // generate tokens
            var (accessToken, accessExpires) = GenerateJwtToken(user);
            var (refreshTokenPlain, refreshExpires, refreshHash) = GenerateRefreshToken();

            // store hash + expiry
            user.RefreshTokenHash = refreshHash;
            user.RefreshTokenExpiresAt = refreshExpires;
            await _userRepo.UpdateUserAsync(user);
            await _userRepo.SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessToken,
                AccessTokenExpiresAt = accessExpires,
                RefreshToken = refreshTokenPlain, // send plain token to client once
                RefreshTokenExpiresAt = refreshExpires
            };
        }

        private (string token, DateTime expiresAt) GenerateJwtToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = jwtSection["Key"];
            var issuer = jwtSection["Issuer"] ?? "SchoolRunApp";
            var audience = jwtSection["Audience"] ?? "SchoolRunAppClients";
            var expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var m) ? m : 60;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }
        public async Task<bool> RequestPasswordResetAsync(RequestPasswordResetDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email.Trim());
            if (user == null)
                return false;

            var resetCode = Guid.NewGuid().ToString("N")[..6].ToUpper();

            user.PasswordResetCode = resetCode;
            user.PasswordResetExpiry = DateTime.UtcNow.AddMinutes(10);

            await _userRepo.UpdateUserAsync(user);
            await _userRepo.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                user.Email,
                "Password Reset Request",
                $@"<h2>Hello {user.FullName}</h2>
           <p>Your password reset code is:</p>
           <h1>{resetCode}</h1>
           <p>This code expires in 10 minutes.</p>"
            );

            return true;
        }
    public async Task<bool> VerifyResetCodeAsync(VerifyResetCodeDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email.Trim());
        if (user == null) return false;

        if (user.PasswordResetCode != dto.ResetCode)
            return false;

        if (user.PasswordResetExpiry == null || user.PasswordResetExpiry < DateTime.UtcNow)
            return false;

        return true;
    }
    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
{
    var user = await _userRepo.GetByEmailAsync(dto.Email.Trim());
    if (user == null) return false;

    if (user.PasswordResetCode != dto.ResetCode)
        return false;

    if (user.PasswordResetExpiry == null || user.PasswordResetExpiry < DateTime.UtcNow)
        return false;

    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

    user.PasswordResetCode = null;
    user.PasswordResetExpiry = null;

    await _userRepo.UpdateUserAsync(user);
    await _userRepo.SaveChangesAsync();

    return true;
}
 public async Task<AuthResponseDto> RefreshTokenAsync(RefreshRequestDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var user = await _userRepo.GetByEmailAsync(dto.Email.Trim());
            if (user == null) throw new InvalidOperationException("Invalid refresh request.");

            if (user.RefreshTokenHash == null || user.RefreshTokenExpiresAt == null || user.RefreshTokenExpiresAt < DateTime.UtcNow)
                throw new InvalidOperationException("Refresh token expired or invalid.");

            // validate provided token by hashing and comparing
            var providedHash = ComputeSha256Hash(dto.RefreshToken);
            if (!CryptographicEquals(providedHash, user.RefreshTokenHash))
                throw new InvalidOperationException("Invalid refresh token.");

            // rotate refresh token: issue new refresh token and access token
            var (accessToken, accessExpires) = GenerateJwtToken(user);
            var (newRefreshPlain, newRefreshExpires, newRefreshHash) = GenerateRefreshToken();

            user.RefreshTokenHash = newRefreshHash;
            user.RefreshTokenExpiresAt = newRefreshExpires;
            await _userRepo.UpdateUserAsync(user);
            await _userRepo.SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role,
                AccessToken = accessToken,
                AccessTokenExpiresAt = accessExpires,
                RefreshToken = newRefreshPlain,
                RefreshTokenExpiresAt = newRefreshExpires
            };
        }

        public async Task<bool> RevokeRefreshTokenAsync(RevokeRefreshDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var user = await _userRepo.GetByEmailAsync(dto.Email.Trim());
            if (user == null) return false;

            // Optional: verify token matches before revoking
            if (!string.IsNullOrWhiteSpace(dto.RefreshToken) && !string.IsNullOrEmpty(user.RefreshTokenHash))
            {
                var providedHash = ComputeSha256Hash(dto.RefreshToken);
                if (!CryptographicEquals(providedHash, user.RefreshTokenHash))
                {
                    // not matching — we can still clear tokens to be safe
                    user.RefreshTokenHash = null;
                    user.RefreshTokenExpiresAt = null;
                }
                else
                {
                    user.RefreshTokenHash = null;
                    user.RefreshTokenExpiresAt = null;
                }
            }
            else
            {
                // clear anyway
                user.RefreshTokenHash = null;
                user.RefreshTokenExpiresAt = null;
            }

            await _userRepo.UpdateUserAsync(user);
            await _userRepo.SaveChangesAsync();
            return true;
        }
         private (string plainToken, DateTime expiresAt, string hash) GenerateRefreshToken()
        {
          
            var randomBytes = new byte[64];
            RandomNumberGenerator.Fill(randomBytes);
            var plain = Convert.ToBase64String(randomBytes);
            var expiresAt = DateTime.UtcNow.AddMinutes(_refreshTokenMinutes);
            var hash = ComputeSha256Hash(plain);
            return (plain, expiresAt, hash);
        }

        private static string ComputeSha256Hash(string raw)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(raw);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // time-constant comparison
        private static bool CryptographicEquals(string a, string b)
        {
            var ba = Convert.FromBase64String(a);
            var bb = Convert.FromBase64String(b);
            if (ba.Length != bb.Length) return false;
            var diff = 0;
            for (int i = 0; i < ba.Length; i++) diff |= ba[i] ^ bb[i];
            return diff == 0;
        }

    }
}
