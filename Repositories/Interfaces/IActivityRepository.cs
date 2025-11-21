using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        Task<IEnumerable<Activity>> GetAllAsync();
        Task<Activity?> GetByIdAsync(int id);
        Task AddAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
