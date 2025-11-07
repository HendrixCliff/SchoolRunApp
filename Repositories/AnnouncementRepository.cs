using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly AppDbContext _context;
        public AnnouncementRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Announcement>> GetAllAsync() =>
            await _context.Announcements.OrderByDescending(a => a.DatePosted).ToListAsync();

        public async Task<Announcement?> GetByIdAsync(int id) =>
            await _context.Announcements.FindAsync(id);

        public async Task AddAsync(Announcement a) => await _context.Announcements.AddAsync(a);
        public async Task UpdateAsync(Announcement a) => _context.Announcements.Update(a);
        public async Task DeleteAsync(int id)
        {
            var a = await GetByIdAsync(id);
            if (a != null) _context.Announcements.Remove(a);
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
