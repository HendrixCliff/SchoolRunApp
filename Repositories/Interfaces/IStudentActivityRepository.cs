using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolRunApp.API.Models;

namespace SchoolRunApp.API.Repositories.Interfaces
{
    public interface IStudentActivityRepository
    {
        Task<IEnumerable<Activity>> GetActivitiesForStudentAsync(int studentId);
        Task<bool> IsStudentInActivityAsync(int studentId, int activityId);
        Task AddStudentToActivityAsync(StudentActivity studentActivity);
        Task RemoveStudentFromActivityAsync(int studentId, int activityId);
        Task SaveChangesAsync();
    }
}
