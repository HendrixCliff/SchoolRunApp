using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.Services.Interfaces;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.DTOs.Student;

namespace SchoolRunApp.API.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;
        private readonly IClassRepository _classRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<StudentService> _logger;

        public StudentService(
            IStudentRepository repo,
            IClassRepository classRepo,
            IUserRepository userRepo,
            ILogger<StudentService> logger)
        {
            _repo = repo;
            _classRepo = classRepo;
            _userRepo = userRepo;
            _logger = logger;
        }

        
        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _repo.GetAllStudentsAsync();

            return students.Select(s => new StudentDto
            {
                Id = s.Id,
                FullName = s.User?.FullName ?? "",
                AdmissionNumber = s.AdmissionNumber,
                ClassId = s.ClassId,
                ClassName = s.Class?.ClassName ?? "",
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
                Address = s.Address,
                UserId = s.UserId
            });
        }

        
        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            var s = await _repo.GetStudentByIdAsync(id);
            if (s == null) return null;

            return new StudentDto
            {
                Id = s.Id,
                FullName = s.User?.FullName ?? "",
                AdmissionNumber = s.AdmissionNumber,
                ClassId = s.ClassId,
                ClassName = s.Class?.ClassName ?? "",
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
                Address = s.Address,
                UserId = s.UserId
            };
        }

    
        public async Task<bool> CreateStudentAsync(CreateStudentDto dto)
        {
          
            var existing = await _repo.GetByAdmissionNumberAsync(dto.AdmissionNumber);
            if (existing != null)
                throw new InvalidOperationException("Admission number already exists.");

           
            var classEntity = await _classRepo.GetByIdAsync(dto.ClassId);
            if (classEntity == null)
                throw new InvalidOperationException("Class not found.");

         
            var newUser = new User
            {
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim(),
                Role = "Student",
                IsActive = false,
                TempActivationCode = Guid.NewGuid().ToString("N").Substring(0, 6)
            };

            await _userRepo.AddUserAsync(newUser);
            await _userRepo.SaveChangesAsync();

           
            var student = new StudentProfile
            {
                AdmissionNumber = dto.AdmissionNumber,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Address = dto.Address ?? "N/A",
                ClassId = dto.ClassId,
                UserId = newUser.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddStudentAsync(student);
            await _repo.SaveChangesAsync();

            // TODO: Send Activation Email (Plug Email Service)

            return true;
        }

        
        public async Task<bool> ActivateStudentAsync(ActivateStudentDto dto)
        {
            var student = await _repo.GetByAdmissionNumberAsync(dto.AdmissionNumber);
            if (student == null) return false;

            var user = await _userRepo.GetByIdAsync(student.UserId);
            if (user == null) return false;

            if (user.TempActivationCode != dto.ActivationCode)
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.IsActive = true;
            user.TempActivationCode = null;

            await _userRepo.UpdateUserAsync(user);
            await _userRepo.SaveChangesAsync();

            return true;
        }

        
        public async Task<bool> UpdateStudentAsync(int id, UpdateStudentDto dto)
        {
            var student = await _repo.GetStudentByIdAsync(id);
            if (student == null) return false;

            student.Gender = dto.Gender;
            student.Address = dto.Address;
            student.DateOfBirth = dto.DateOfBirth;
            student.ClassId = dto.ClassId;

            await _repo.UpdateStudentAsync(student);
            await _repo.SaveChangesAsync();

            return true;
        }

    
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _repo.GetStudentByIdAsync(id);
            if (student == null) return false;

            await _repo.DeleteStudentAsync(id);
            await _repo.SaveChangesAsync();

            return true;
        }
    }
}
