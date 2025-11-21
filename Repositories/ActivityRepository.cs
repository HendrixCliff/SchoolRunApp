using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Data;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Repositories.Interfaces;

namespace SchoolRunApp.API.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;

        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Activity>> GetAllAsync()
        {
            return await _context.Activities.AsNoTracking().ToListAsync();
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities.FindAsync(id);
        }

        public async Task AddAsync(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
        }

        public async Task UpdateAsync(Activity activity)
        {
            _context.Activities.Update(activity);
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await GetByIdAsync(id);
            if (activity != null)
                _context.Activities.Remove(activity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
