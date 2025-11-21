using SchoolRunApp.API.DTOs.Activity;
using SchoolRunApp.API.Models;
using SchoolRunApp.API.Services.Interfaces;
using SchoolRunApp.API.Repositories.Interfaces;

namespace SchoolRunApp.API.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repo;
        private readonly IStudentActivityRepository _studentActivityRepo;

        public ActivityService(
            IActivityRepository repo,
            IStudentActivityRepository studentActivityRepo)
        {
            _repo = repo;
            _studentActivityRepo = studentActivityRepo;
        }

        public async Task<IEnumerable<ActivityDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(a => new ActivityDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            });
        }

        public async Task<bool> JoinActivityAsync(int studentId, int activityId)
        {
            if (await _studentActivityRepo.IsStudentInActivityAsync(studentId, activityId))
                return false;

            var entry = new StudentActivity
            {
                StudentId = studentId,
                ActivityId = activityId
            };

            await _studentActivityRepo.AddStudentToActivityAsync(entry);
            await _studentActivityRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LeaveActivityAsync(int studentId, int activityId)
        {
            await _studentActivityRepo.RemoveStudentFromActivityAsync(studentId, activityId);
            await _studentActivityRepo.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ActivityDto>> GetStudentActivitiesAsync(int studentId)
        {
            var activities = await _studentActivityRepo.GetActivitiesForStudentAsync(studentId);

            return activities.Select(a => new ActivityDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            });
        }
    }
}
