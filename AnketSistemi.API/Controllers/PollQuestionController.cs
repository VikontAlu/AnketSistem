using AnketSistemi.API.Data;
using AnketSistemi.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnketSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Sadece admin soru ekleyebilir güvenliği!
    public class PollQuestionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PollQuestionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] PollQuestion question)
        {
            if (question == null) return BadRequest("Soru verisi boş olamaz.");

            _context.PollQuestions.Add(question);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Soru ve seçenekler başarıyla eklendi!" });
        }
    }
}