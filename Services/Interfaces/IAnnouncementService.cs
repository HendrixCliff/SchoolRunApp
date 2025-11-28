using SchoolRunApp.API.DTOs;
using SchoolRunApp.API.DTOs.Announcement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services.Interfaces
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<AnnouncementDto>> GetAllAsync();
        Task<AnnouncementDto?> GetByIdAsync(int id);
        Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto);
        Task<bool> UpdateAsync(int id, UpdateAnnouncementDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
