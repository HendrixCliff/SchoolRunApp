using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.Services.Interfaces;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.DTOs.Student;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;

        public StudentService(IStudentRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _repo.GetAllStudentsAsync();
            return students.Select(s => new StudentDto
            {
                Id = s.Id,
                FullName = s.User?.FullName ?? "",
                AdmissionNumber = s.AdmissionNumber,
                ClassName = s.Class?.ClassName ?? "",
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
                FullName = s.User?.FullName ?? "",
                AdmissionNumber = s.AdmissionNumber,
                ClassName = s.Class?.ClassName ?? "",
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth
            };
        }

        public async Task<StudentDto> CreateStudentAsync(StudentDto dto)
        {
            var student = new StudentProfile
            {
                AdmissionNumber = dto.AdmissionNumber,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Address = "N/A", // could come later
                ClassId = 1, // default class for now
                UserId = 0 // user linked later
            };

            await _repo.AddStudentAsync(student);
            await _repo.SaveChangesAsync();

            dto.Id = student.Id;
            return dto;
        }

        public async Task<bool> UpdateStudentAsync(int id, StudentDto dto)
        {
            var existing = await _repo.GetStudentByIdAsync(id);
            if (existing == null) return false;

            existing.AdmissionNumber = dto.AdmissionNumber;
            existing.Gender = dto.Gender;
            existing.DateOfBirth = dto.DateOfBirth;

            await _repo.UpdateStudentAsync(existing);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            await _repo.DeleteStudentAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
