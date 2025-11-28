using SchoolRunApp.API.DTOs.Result;

namespace SchoolRunApp.API.Services.Interfaces
{
    public interface IResultService
    {
        Task<IEnumerable<ResultDto>> GetAllAsync();
        Task<ResultDto?> GetByIdAsync(int id);
        Task<ResultDto> CreateAsync(CreateResultDto dto);
        Task<bool> UpdateAsync(int id, UpdateResultDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
