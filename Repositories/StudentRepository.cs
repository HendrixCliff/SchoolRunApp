using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Repositories.Interfaces;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentProfile>> GetAllStudentsAsync()
        {
            return await _context.StudentProfiles
                .Include(s => s.Class)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<StudentProfile?> GetStudentByIdAsync(int id)
        {
            return await _context.StudentProfiles
                .Include(s => s.Class)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<StudentProfile?> GetByAdmissionNumberAsync(string admissionNumber)
        {
            return await _context.StudentProfiles
                .Include(s => s.Class)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.AdmissionNumber == admissionNumber);
        }

        public async Task AddStudentAsync(StudentProfile student)
        {
            await _context.StudentProfiles.AddAsync(student);
        }

        public async Task UpdateStudentAsync(StudentProfile student)
        {
            _context.StudentProfiles.Update(student);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await GetStudentByIdAsync(id);
            if (student != null)
                _context.StudentProfiles.Remove(student);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
