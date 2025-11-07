using SchoolRunApp.API.Data.Repositories.Interfaces;
using SchoolRunApp.API.DTOs;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _repo;
        public ResultService(IResultRepository repo) => _repo = repo;

        public async Task<IEnumerable<ResultDto>> GetAllAsync()
        {
            var results = await _repo.GetAllAsync();
            return results.Select(r => new ResultDto
            {
                Id = r.Id,
                StudentId = r.StudentId,
                StudentName = r.Student?.User?.FullName ?? "",
                SubjectName = r.Subject?.SubjectName ?? "",
                Score = r.Score,
                Term = r.Term,
                Session = r.Session
            });
        }

        public async Task<ResultDto?> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return null;

            return new ResultDto
            {
                Id = r.Id,
                StudentId = r.StudentId,
                StudentName = r.Student?.User?.FullName ?? "",
                SubjectName = r.Subject?.SubjectName ?? "",
                Score = r.Score,
                Term = r.Term,
                Session = r.Session
            };
        }

        public async Task<ResultDto> CreateAsync(ResultDto dto)
        {
            var result = new Result
            {
                StudentId = dto.StudentId,
                SubjectId = 0, // you may set this properly in controller
                Score = dto.Score,
                Term = dto.Term,
                Session = dto.Session
            };
            await _repo.AddAsync(result);
            await _repo.SaveChangesAsync();
            dto.Id = result.Id;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, ResultDto dto)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return false;

            r.Score = dto.Score;
            r.Term = dto.Term;
            r.Session = dto.Session;
            await _repo.UpdateAsync(r);
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
