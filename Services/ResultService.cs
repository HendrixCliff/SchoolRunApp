using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.DTOs.Result;
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

        public ResultService(IResultRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ResultDto>> GetAllAsync()
        {
            var results = await _repo.GetAllAsync();
            return results.Select(r => new ResultDto
            {
                Id = r.Id,
                StudentId = r.StudentId,
                StudentName = r.Student?.User?.FullName ?? "",
                SubjectId = r.SubjectId,
                SubjectName = r.Subject?.SubjectName ?? "",
                ClassId = r.ClassId,
                ClassName = r.Class?.ClassName ?? "",
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
                SubjectId = r.SubjectId,
                SubjectName = r.Subject?.SubjectName ?? "",
                ClassId = r.ClassId,
                ClassName = r.Class?.ClassName ?? "",
                Score = r.Score,
                Term = r.Term,
                Session = r.Session
            };
        }

        public async Task<ResultDto> CreateAsync(CreateResultDto dto)
        {
            var result = new Result
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                ClassId = dto.ClassId,
                Score = dto.Score,
                Term = dto.Term,
                Session = dto.Session
            };

            await _repo.AddAsync(result);
            await _repo.SaveChangesAsync();

            return await GetByIdAsync(result.Id) ?? throw new Exception("Error creating result.");
        }

        public async Task<bool> UpdateAsync(int id, UpdateResultDto dto)
        {
            var result = await _repo.GetByIdAsync(id);
            if (result == null) return false;

            result.Score = dto.Score;
            result.Term = dto.Term;
            result.Session = dto.Session;

            await _repo.UpdateAsync(result);
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
