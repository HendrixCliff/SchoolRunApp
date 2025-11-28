using Microsoft.AspNetCore.Mvc;
using SchoolRunApp.API.DTOs.Subject;
using SchoolRunApp.API.Services.Interfaces;
using SchoolRunApp.API.Services;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _subjectService.GetAllAsync();
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null) return NotFound(new { message = "Subject not found" });
            return Ok(subject);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubjectDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _subjectService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSubjectDto dto)
        {
            var updated = await _subjectService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { message = "Subject not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _subjectService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Subject not found" });
            return NoContent();
        }
    }
}
