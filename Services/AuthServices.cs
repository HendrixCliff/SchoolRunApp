using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SchoolRunApp.API.DTOs.Auth;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Repositories;
using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.Services.Interfaces;
using BCrypt.Net;

namespace SchoolRunApp.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepo,
            IStudentRepository studentRepo,
            ITeacherRepository teacherRepo,
            IConfiguration config,
            ILogger<AuthService> logger)
        {
            _userRepo = userRepo;
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
            _config = config;
            _logger = logger;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var emailExists = await _userRepo.GetByEmailAsync(dto.Email);
            if (emailExists != null)
                throw new InvalidOperationException("Email already in use.");

            // Only allow Student or Teacher role registration here
            if (dto.Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(dto.AdmissionNumber))
                    throw new ArgumentException("AdmissionNumber is required for student registration.");

                var student = await _studentRepo.GetByAdmissionNumberAsync(dto.AdmissionNumber.Trim());
                if (student == null)
                    throw new InvalidOperationException("Admission number not found in records.");

                if (student.UserId != 0)
                    throw new InvalidOperationException("An account already exists for this student.");

                var user = new User
                {
                    FullName = dto.FullName,
                    Email = dto.Email.Trim(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = "Student"
                };

                await _userRepo.AddUserAsync(user);
                await _userRepo.SaveChangesAsync();

                student.UserId = user.Id;
                await _studentRepo.UpdateStudentAsync(student);
                await _studentRepo.SaveChangesAsync();

                var token = GenerateJwtToken(user);
                return new AuthResponseDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    Token = token.token,
                    ExpiresAt = token.expiresAt
                };
            }

            if (dto.Role.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(dto.TeacherId))
                    throw new ArgumentException("TeacherId is required for teacher registration.");

                var teacher = await _teacherRepo.GetByTeacherCodeAsync(dto.TeacherId.Trim());
                if (teacher == null)
                    throw new InvalidOperationException("Teacher ID not found in records.");

                if (teacher.UserId != 0)
                    throw new InvalidOperationException("An account already exists for this teacher.");

                var user = new User
                {
                    FullName = dto.FullName,
                    Email = dto.Email.Trim(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = "Teacher"
                };

                await _userRepo.AddUserAsync(user);
                await _userRepo.SaveChangesAsync();

                teacher.UserId = user.Id;
                await _teacherRepo.UpdateAsync(teacher);
                await _teacherRepo.SaveChangesAsync();

                var token = GenerateJwtToken(user);
                return new AuthResponseDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    Token = token.token,
                    ExpiresAt = token.expiresAt
                };
            }

            // Admin or others should be created only by existing admin accounts
            throw new InvalidOperationException("Registration for this role is not allowed via public endpoint.");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var user = await _userRepo.GetByEmailAsync(dto.Email.Trim());
            if (user == null)
                throw new InvalidOperationException("Invalid credentials.");

            var ok = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!ok) throw new InvalidOperationException("Invalid credentials.");

            var token = GenerateJwtToken(user);
            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Role = user.Role,
                Token = token.token,
                ExpiresAt = token.expiresAt
            };
        }

        // returns token and expiry
        private (string token, DateTime expiresAt) GenerateJwtToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key missing in config");
            var issuer = jwtSection["Issuer"] ?? "SchoolRunApp";
            var audience = jwtSection["Audience"] ?? "SchoolRunAppClients";
            var expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var m) ? m : 60;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
            };

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expiresAt);
        }
    }
}
