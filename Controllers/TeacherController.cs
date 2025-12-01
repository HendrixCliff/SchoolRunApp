using Microsoft.AspNetCore.Mvc;
using SchoolRunApp.API.Services.Interfaces;
using SchoolRunApp.API.DTOs.Teacher;

namespace SchoolRunApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _service;

        public TeacherController(ITeacherService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _service.GetByIdAsync(id);
            return teacher == null ? NotFound() : Ok(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeacherDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(new { message = "Teacher created. Activation code sent.", result });
        }

        
        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody] ActivateTeacherDto dto)
        {
            var ok = await _service.ActivateTeacherAsync(dto);
            return ok
                ? Ok(new { message = "Account activated successfully!" })
                : BadRequest(new { message = "Invalid teacher code or activation code" });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateTeacherDto dto)
        {
            var ok = await _service.UpdateAsync(id, dto);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? Ok() : NotFound();
        }
    }
}
