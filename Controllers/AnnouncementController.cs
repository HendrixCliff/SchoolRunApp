using Microsoft.AspNetCore.Mvc;
using SchoolRunApp.API.DTOs;
using SchoolRunApp.API.Services.Interfaces;
using System.Threading.Tasks;

namespace SchoolRunApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var announcements = await _announcementService.GetAllAsync();
            return Ok(announcements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var announcement = await _announcementService.GetByIdAsync(id);
            if (announcement == null) return NotFound(new { message = "Announcement not found" });
            return Ok(announcement);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AnnouncementDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _announcementService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AnnouncementDto dto)
        {
            var updated = await _announcementService.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { message = "Announcement not found" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _announcementService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Announcement not found" });
            return NoContent();
        }
    }
}
