using Microsoft.AspNetCore.Mvc;
using SchoolRunApp.API.DTOs;
using SchoolRunApp.API.Services.Interfaces;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var results = await _resultService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _resultService.GetByIdAsync(id);
            if (result == null) return NotFound(new { message = "Result not found" });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ResultDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _resultService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ResultDto dto)
        {
            var updated = await _resultService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { message = "Result not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _resultService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Result not found" });
            return NoContent();
        }
    }
}
