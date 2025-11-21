using Microsoft.AspNetCore.Mvc;
using SchoolRunApp.API.DTOs.Activity;
using SchoolRunApp.API.Services.Interfaces;

namespace SchoolRunApp.API.Controllers
{
    [ApiController]
    [Route("api/activities")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivityController(IActivityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost("{studentId}/join/{activityId}")]
        public async Task<IActionResult> JoinActivity(int studentId, int activityId)
        {
            var success = await _service.JoinActivityAsync(studentId, activityId);
            if (!success)
                return BadRequest("Student already joined this activity.");

            return Ok("Joined successfully.");
        }

        [HttpDelete("{studentId}/leave/{activityId}")]
        public async Task<IActionResult> LeaveActivity(int studentId, int activityId)
        {
            await _service.LeaveActivityAsync(studentId, activityId);
            return Ok("Left activity.");
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentActivities(int studentId)
        {
            return Ok(await _service.GetStudentActivitiesAsync(studentId));
        }
    }
}
