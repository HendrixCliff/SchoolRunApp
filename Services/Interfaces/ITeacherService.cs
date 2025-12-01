using SchoolRunApp.API.DTOs.Teacher;

namespace SchoolRunApp.API.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetAllAsync();
        Task<TeacherDto?> GetByIdAsync(int id);
        Task<TeacherDto> CreateAsync(CreateTeacherDto dto);
        Task<bool> ActivateTeacherAsync(ActivateTeacherDto dto);
        Task<bool> UpdateAsync(int id, UpdateTeacherDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
