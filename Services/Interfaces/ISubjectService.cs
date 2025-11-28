using SchoolRunApp.API.DTOs.Subject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectDto>> GetAllAsync();
        Task<SubjectDto?> GetByIdAsync(int id);
        Task<SubjectDto> CreateAsync(CreateSubjectDto dto);
        Task<bool> UpdateAsync(int id, UpdateSubjectDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
