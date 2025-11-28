using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _context;
        public SubjectRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Subject>> GetAllAsync() => await _context.Subjects.ToListAsync();
        public async Task<Subject?> GetByIdAsync(int id) => await _context.Subjects.FindAsync(id);
        public async Task<Subject?> GetByCodeAsync(string code)
            {
                return await _context.Subjects
                    .FirstOrDefaultAsync(s => s.Code == code);
            }

        public async Task AddAsync(Subject subject) => await _context.Subjects.AddAsync(subject);
        public async Task UpdateAsync(Subject subject) => _context.Subjects.Update(subject);
        public async Task DeleteAsync(int id)
        {
            var s = await GetByIdAsync(id);
            if (s != null) _context.Subjects.Remove(s);
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
