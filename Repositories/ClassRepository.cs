using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Data;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Repositories.Interfaces;

namespace SchoolRunApp.API.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            return await _context.Classes
                .Include(c => c.Students)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _context.Classes
                .Include(c => c.Students)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Class?> GetByNameAsync(string className)
        {
            return await _context.Classes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ClassName == className);
        }

        public async Task AddAsync(Class newClass)
        {
            await _context.Classes.AddAsync(newClass);
        }

        public async Task UpdateAsync(Class existingClass)
        {
            _context.Classes.Update(existingClass);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Classes.FindAsync(id);
            if (entity != null)
                _context.Classes.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
