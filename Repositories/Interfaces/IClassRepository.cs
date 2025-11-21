using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Repositories.Interfaces
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(int id);
        Task<Class?> GetByNameAsync(string className);
        Task AddAsync(Class newClass);
        Task UpdateAsync(Class existingClass);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
