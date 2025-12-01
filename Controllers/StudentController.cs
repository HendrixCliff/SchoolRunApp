using Microsoft.AspNetCore.Mvc;
using SchoolRunApp.API.Services.Interfaces;
using SchoolRunApp.API.DTOs.Student;

namespace SchoolRunApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllStudentsAsync();
            return Ok(result);
        }

       
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _service.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound(new { message = "Student not found" });

            return Ok(student);
        }


        // CREATE STUDENT (admin/staff creates profile)
        // Auto-creates User + Sends Activation code
       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.CreateStudentAsync(dto);

            return success
                ? Ok(new { message = "Student created and activation code sent." })
                : BadRequest(new { message = "Unable to create student" });
        }

      
        // ACTIVATE STUDENT ACCOUNT
        // Student uses admission number + activation code + password
       
        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody] ActivateStudentDto dto)
        {
            var success = await _service.ActivateStudentAsync(dto);

            return success
                ? Ok(new { message = "Account activated successfully!" })
                : BadRequest(new { message = "Invalid activation code or admission number" });
        }

       
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto dto)
        {
            var success = await _service.UpdateStudentAsync(id, dto);

            return success
                ? Ok(new { message = "Student updated successfully" })
                : NotFound(new { message = "Student not found" });
        }

        // -----------------------------------------
        // DELETE STUDENT
        // -----------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteStudentAsync(id);

            return success
                ? Ok(new { message = "Student deleted successfully" })
                : NotFound(new { message = "Student not found" });
        }
    }
}
