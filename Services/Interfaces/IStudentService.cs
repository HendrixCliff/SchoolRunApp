using SchoolRunApp.API.DTOs.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentByIdAsync(int id);
        Task<StudentDto> CreateStudentAsync(StudentDto dto);
        Task<bool> UpdateStudentAsync(int id, StudentDto dto);
        Task<bool> DeleteStudentAsync(int id);
    }
}
