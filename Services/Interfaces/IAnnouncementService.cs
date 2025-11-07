using SchoolRunApp.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services.Interfaces
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<AnnouncementDto>> GetAllAsync();
        Task<AnnouncementDto?> GetByIdAsync(int id);
        Task<AnnouncementDto> CreateAsync(AnnouncementDto dto);
        Task<bool> UpdateAsync(int id, AnnouncementDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
