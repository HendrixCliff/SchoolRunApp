using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Data;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext _context;

        public TeacherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherProfile>> GetAllAsync()
        {
            return await _context.Set<TeacherProfile>().AsNoTracking().ToListAsync();
        }

        public async Task<TeacherProfile?> GetByIdAsync(int id)
        {
            return await _context.Set<TeacherProfile>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TeacherProfile?> GetByTeacherCodeAsync(string teacherCode)
        {
            return await _context.Set<TeacherProfile>().AsNoTracking()
                .FirstOrDefaultAsync(t => t.TeacherCode == teacherCode);
        }

        public async Task AddAsync(TeacherProfile teacher)
        {
            await _context.Set<TeacherProfile>().AddAsync(teacher);
        }

        public async Task UpdateAsync(TeacherProfile teacher)
        {
            _context.Set<TeacherProfile>().Update(teacher);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<TeacherProfile>().FindAsync(id);
            if (entity != null) _context.Set<TeacherProfile>().Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
