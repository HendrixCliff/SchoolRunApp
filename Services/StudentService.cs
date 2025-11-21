using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.Services.Interfaces;
using SchoolRunApp.API.Services;
using SchoolRunApp.API.Repositories;
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
                FullName = s.User?.FullName ?? string.Empty,
                AdmissionNumber = s.AdmissionNumber,
                ClassName = s.Class?.ClassName ?? string.Empty,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth
            });
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            var s = await _repo.GetStudentByIdAsync(id);
            if (s == null) return null;

            return new StudentDto
            {
                Id = s.Id,
                FullName = s.User?.FullName ?? string.Empty,
                AdmissionNumber = s.AdmissionNumber,
                ClassName = s.Class?.ClassName ?? string.Empty,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth
            };
        }

        public async Task<StudentDto> CreateStudentAsync(StudentDto dto)
        {
            // ✅ Validate input
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Student data cannot be null.");
            if (string.IsNullOrWhiteSpace(dto.AdmissionNumber))
                throw new ArgumentException("Admission number is required.", nameof(dto.AdmissionNumber));

            // ✅ Check duplicates
            var existing = await _repo.GetByAdmissionNumberAsync(dto.AdmissionNumber);
            if (existing != null)
                throw new InvalidOperationException($"A student with admission number {dto.AdmissionNumber} already exists.");

            // ✅ Verify class
            var targetClass = await _classRepo.GetByIdAsync(dto.ClassId);
            if (targetClass == null)
            {
                _logger.LogWarning("Class with ID {ClassId} not found. Defaulting to class ID 1.", dto.ClassId);
                dto.ClassId = 1;
            }

            // ✅ Verify user if provided
            if (dto.UserId.HasValue)
            {
                var user = await _userRepo.GetByIdAsync(dto.UserId.Value);
                if (user == null)
                    throw new InvalidOperationException($"User with ID {dto.UserId.Value} does not exist.");
            }

            var student = new StudentProfile
            {
                AdmissionNumber = dto.AdmissionNumber.Trim(),
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Address = string.IsNullOrWhiteSpace(dto.Address) ? "N/A" : dto.Address.Trim(),
                ClassId = dto.ClassId,
                UserId = dto.UserId ?? 0,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _repo.AddStudentAsync(student);
                await _repo.SaveChangesAsync();

                dto.Id = student.Id;
                _logger.LogInformation("Student {AdmissionNumber} created successfully with ID {Id}", dto.AdmissionNumber, student.Id);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student {AdmissionNumber}", dto.AdmissionNumber);
                throw new ApplicationException("An error occurred while creating the student. Please try again.", ex);
            }
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentDto dto)
        {
            var existing = await _repo.GetStudentByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Student with ID {Id} not found for update.", id);
                return false;
            }

            existing.AdmissionNumber = dto.AdmissionNumber;
            existing.Gender = dto.Gender;
            existing.DateOfBirth = dto.DateOfBirth;
            existing.Address = dto.Address ?? existing.Address;

            try
            {
                await _repo.UpdateStudentAsync(existing);
                await _repo.SaveChangesAsync();
                _logger.LogInformation("Student with ID {Id} updated successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student with ID {Id}", id);
                throw new ApplicationException("An error occurred while updating the student.", ex);
            }
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var existing = await _repo.GetStudentByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Attempted to delete non-existent student with ID {Id}.", id);
                return false;
            }

            try
            {
                await _repo.DeleteStudentAsync(id);
                await _repo.SaveChangesAsync();
                _logger.LogInformation("Student with ID {Id} deleted successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student with ID {Id}", id);
                throw new ApplicationException("An error occurred while deleting the student.", ex);
            }
        }
    }
}
