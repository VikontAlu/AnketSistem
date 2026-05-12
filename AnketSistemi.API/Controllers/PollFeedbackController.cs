using AnketSistemi.API.Data;
using AnketSistemi.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnketSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece sisteme giriş yapmış kullanıcılar puan verebilir
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
            // İsteği atan kullanıcının ID'sini JWT Token içinden otomatik yakalıyoruz! (Tam bir profesyonel hareketi)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            feedback.UserId = userId; // Token'dan gelen güvenli ID'yi basıyoruz

            _context.PollFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Değerlendirmeniz başarıyla kaydedildi!" });
        }
    }
}