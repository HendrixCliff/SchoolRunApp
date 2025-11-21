using Microsoft.EntityFrameworkCore;
using SchoolRunApp.API.Data;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Repositories.Interfaces;

namespace SchoolRunApp.API.Repositories
{
    public class StudentActivityRepository : IStudentActivityRepository
    {
        private readonly AppDbContext _context;

        public StudentActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Activity>> GetActivitiesForStudentAsync(int studentId)
        {
            return await _context.StudentActivities
                .Where(sa => sa.StudentId == studentId)
                .Select(sa => sa.Activity)
                .ToListAsync();
        }

        public async Task<bool> IsStudentInActivityAsync(int studentId, int activityId)
        {
            return await _context.StudentActivities
                .AnyAsync(sa => sa.StudentId == studentId && sa.ActivityId == activityId);
        }

        public async Task AddStudentToActivityAsync(StudentActivity studentActivity)
        {
            await _context.StudentActivities.AddAsync(studentActivity);
        }

        public async Task RemoveStudentFromActivityAsync(int studentId, int activityId)
        {
            var record = await _context.StudentActivities
                .FirstOrDefaultAsync(sa => sa.StudentId == studentId && sa.ActivityId == activityId);

            if (record != null)
                _context.StudentActivities.Remove(record);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
