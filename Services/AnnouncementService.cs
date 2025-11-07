using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.DTOs;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _repo;
        public AnnouncementService(IAnnouncementRepository repo) => _repo = repo;

        public async Task<IEnumerable<AnnouncementDto>> GetAllAsync()
        {
            var announcements = await _repo.GetAllAsync();
            return announcements.Select(a => new AnnouncementDto
            {
                Id = a.Id,
                Title = a.Title,
                Message = a.Message,
                DatePosted = a.DatePosted,
                PostedBy = a.PostedBy
            });
        }

        public async Task<AnnouncementDto?> GetByIdAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return null;

            return new AnnouncementDto
            {
                Id = a.Id,
                Title = a.Title,
                Message = a.Message,
                DatePosted = a.DatePosted,
                PostedBy = a.PostedBy
            };
        }

        public async Task<AnnouncementDto> CreateAsync(AnnouncementDto dto)
        {
            var a = new Announcement
            {
                Title = dto.Title,
                Message = dto.Message,
                PostedBy = dto.PostedBy
            };
            await _repo.AddAsync(a);
            await _repo.SaveChangesAsync();
            dto.Id = a.Id;
            dto.DatePosted = a.DatePosted;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, AnnouncementDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Title = dto.Title;
            existing.Message = dto.Message;
            await _repo.UpdateAsync(existing);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
