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

        public AnnouncementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Announcement>> GetAllAsync()
        {
            return await _context.Announcements
                .Include(a => a.PostedByUser)
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .Include(a => a.Student)
                .ToListAsync();
        }

        public async Task<Announcement?> GetByIdAsync(int id)
        {
            return await _context.Announcements
                .Include(a => a.PostedByUser)
                .Include(a => a.Class)
                .Include(a => a.Subject)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Announcement announcement)
        {
            await _context.Announcements.AddAsync(announcement);
        }

        public async Task UpdateAsync(Announcement announcement)
        {
            _context.Announcements.Update(announcement);
        }

        public async Task DeleteAsync(int id)
        {
            var a = await GetByIdAsync(id);
            if (a != null)
                _context.Announcements.Remove(a);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
