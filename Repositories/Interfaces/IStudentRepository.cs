using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentProfile>> GetAllStudentsAsync();
        Task<StudentProfile?> GetStudentByIdAsync(int id);

        Task<StudentProfile?> GetByAdmissionNumberAsync(string admissionNumber);
        
        Task AddStudentAsync(StudentProfile student);
        Task UpdateStudentAsync(StudentProfile student);
        Task DeleteStudentAsync(int id);
        Task SaveChangesAsync();
    }
}
