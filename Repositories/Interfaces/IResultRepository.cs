using SchoolRunApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Repositories.Interfaces
{
    public interface IResultRepository
    {
        Task<IEnumerable<Result>> GetAllAsync();
        Task<Result?> GetByIdAsync(int id);
        Task AddAsync(Result result);
        Task UpdateAsync(Result result);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
