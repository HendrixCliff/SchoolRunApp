using SchoolRunApp.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services.Interfaces
{
    public interface IResultService
    {
        Task<IEnumerable<ResultDto>> GetAllAsync();
        Task<ResultDto?> GetByIdAsync(int id);
        Task<ResultDto> CreateAsync(ResultDto dto);
        Task<bool> UpdateAsync(int id, ResultDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
