using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly AppDbContext _context;
        public ResultRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Result>> GetAllAsync() =>
            await _context.Results.Include(r => r.Subject).Include(r => r.Student).ThenInclude(s => s.User).ToListAsync();

        public async Task<Result?> GetByIdAsync(int id) =>
            await _context.Results.Include(r => r.Subject).Include(r => r.Student).ThenInclude(s => s.User)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task AddAsync(Result result) => await _context.Results.AddAsync(result);
        public async Task UpdateAsync(Result result) => _context.Results.Update(result);
        public async Task DeleteAsync(int id)
        {
            var r = await GetByIdAsync(id);
            if (r != null) _context.Results.Remove(r);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
