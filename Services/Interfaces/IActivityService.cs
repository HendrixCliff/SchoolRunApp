using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolRunApp.API.DTOs.Activity;

namespace SchoolRunApp.API.Services.Interfaces
{
    public interface IActivityService
    {
       
        /// Returns all school acti
        Task<IEnumerable<ActivityDto>> GetAllAsync();

        Task<bool> JoinActivityAsync(int studentId, int activityId);
    
        Task<bool> LeaveActivityAsync(int studentId, int activityId);

        Task<IEnumerable<ActivityDto>> GetStudentActivitiesAsync(int studentId);
    }
}
