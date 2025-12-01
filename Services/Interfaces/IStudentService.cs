using SchoolRunApp.API.DTOs.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentByIdAsync(int id);
        Task<bool> CreateStudentAsync(CreateStudentDto dto);
        Task<bool> ActivateStudentAsync(ActivateStudentDto dto);
        Task<bool> UpdateStudentAsync(int id, UpdateStudentDto dto);
        Task<bool> DeleteStudentAsync(int id);
    }
}
