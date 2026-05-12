using AnketSistemi.API.Data;
using AnketSistemi.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetFeedbacks([FromQuery] int? pollId)
        {
            var query = _context.PollFeedbacks.AsQueryable();

            if (pollId.HasValue)
                query = query.Where(f => f.PollId == pollId.Value);

            var feedbacks = await query
                .Include(f => f.Poll)
                .Include(f => f.User)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new
                {
                    id = f.Id,
                    pollTitle = f.Poll.Title,
                    userEmail = f.User.Email,
                    score = f.Score,
                    comment = f.UserComment,
                    createdAt = f.CreatedAt
                })
                .ToListAsync();

            return Ok(feedbacks);
        }
    }
}
