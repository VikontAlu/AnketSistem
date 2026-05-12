using AnketSistemi.API.Data;
using AnketSistemi.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnketSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PollFeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PollFeedbackController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] PollFeedback feedback)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Kullanici bulunamadi.");

            feedback.UserId = userId;

            _context.PollFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Degerlendirmeniz basariyla kaydedildi!" });
        }
    }
}
