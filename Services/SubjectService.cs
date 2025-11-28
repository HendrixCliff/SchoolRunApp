using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.DTOs.Subject;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _repo;

        public SubjectService(ISubjectRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SubjectDto>> GetAllAsync()
        {
            var subjects = await _repo.GetAllAsync();

            return subjects.Select(s => new SubjectDto
            {
                Id = s.Id,
                SubjectName = s.SubjectName,
                Code = s.Code
            });
        }

        public async Task<SubjectDto?> GetByIdAsync(int id)
        {
            var s = await _repo.GetByIdAsync(id);

            if (s == null)
                return null;

            return new SubjectDto
            {
                Id = s.Id,
                SubjectName = s.SubjectName,
                Code = s.Code
            };
        }

        public async Task<SubjectDto> CreateAsync(CreateSubjectDto dto)
        {
            // Check for duplicates
            var existing = await _repo.GetByCodeAsync(dto.Code);
            if (existing != null)
                throw new System.Exception($"Subject with code '{dto.Code}' already exists.");

            var subject = new Subject
            {
                SubjectName = dto.SubjectName.Trim(),
                Code = dto.Code.Trim()
            };

            await _repo.AddAsync(subject);
            await _repo.SaveChangesAsync();

            return new SubjectDto
            {
                Id = subject.Id,
                SubjectName = subject.SubjectName,
                Code = subject.Code
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateSubjectDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return false;

            existing.SubjectName = dto.SubjectName.Trim();
            existing.Code = dto.Code.Trim();

            await _repo.UpdateAsync(existing);
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
