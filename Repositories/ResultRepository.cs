using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Data;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Repositories.Interfaces;

namespace SchoolRunApp.API.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly AppDbContext _context;

        public ResultRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Result>> GetAllAsync()
        {
            return await _context.Results
                .Include(r => r.Student).ThenInclude(s => s.User)
                .Include(r => r.Class)
                .Include(r => r.Subject)
                .ToListAsync();
        }

        public async Task<Result?> GetByIdAsync(int id)
        {
            return await _context.Results
                .Include(r => r.Student).ThenInclude(s => s.User)
                .Include(r => r.Class)
                .Include(r => r.Subject)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Result result)
        {
            await _context.Results.AddAsync(result);
        }

        public async Task UpdateAsync(Result result)
        {
            _context.Results.Update(result);
        }

        public async Task DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);
            if (result != null)
                _context.Results.Remove(result);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
