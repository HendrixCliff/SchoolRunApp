using SchoolRunApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Repositories.Interfaces
{
    public interface IAnnouncementRepository
    {
        Task<IEnumerable<Announcement>> GetAllAsync();
        Task<Announcement?> GetByIdAsync(int id);
        Task AddAsync(Announcement announcement);
        Task UpdateAsync(Announcement announcement);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
