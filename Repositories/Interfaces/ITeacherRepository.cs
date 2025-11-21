using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Repositories.Interfaces
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<TeacherProfile>> GetAllAsync();
        Task<TeacherProfile?> GetByIdAsync(int id);
        Task<TeacherProfile?> GetByTeacherCodeAsync(string teacherCode);
        Task AddAsync(TeacherProfile teacher);
        Task UpdateAsync(TeacherProfile teacher);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
