using AnketSistemi.API.DTOs;
using AnketSistemi.API.Models;
using AnketSistemi.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AnketSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResponseController : ControllerBase
    {
        private readonly IGenericRepository<UserResponse> _responseRepo;

        public ResponseController(IGenericRepository<UserResponse> responseRepo)
        {
            _responseRepo = responseRepo;
        }

        // BUG FIX #4: URL artik /api/Response/Submit?pollId=X seklinde cagriliyor
        [HttpPost("Submit")]
        public async Task<IActionResult> Submit([FromQuery] int pollId, [FromBody] List<QuestionAnswerDto> answers, [FromServices] IGenericRepository<Poll> pollRepo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var poll = await pollRepo.GetByIdAsync(pollId);
            if (poll == null || !poll.IsActive)
                return NotFound(new { message = "Anket bulunamadi veya pasif." });

            if (poll.ExpireDate < DateTime.Now)
                return BadRequest(new { status = false, message = "Bu anketin suresi dolmustur, artik cevap verilemez." });

            var alreadyAnswered = await _responseRepo.AsQueryable()
                .AnyAsync(r => r.PollId == pollId && r.AppUserId == userId);

            if (alreadyAnswered)
                return BadRequest(new { status = false, message = "Bu anketi zaten tamamladiniz." });

            foreach (var item in answers)
            {
                var response = new UserResponse
                {
                    PollId = pollId,
                    // BUG FIX #4: DTO field adlari duzeltildi (QuestionId ve SelectedOptionId)
                    PollQuestionId = item.QuestionId,
                    WrittenAnswer = item.TextAnswer,
                    SelectedChoiceId = item.SelectedOptionId,
                    AppUserId = userId!,
                    CreatedAt = DateTime.Now
                };
                await _responseRepo.AddAsync(response);
            }

            await _responseRepo.SaveAsync();
            return Ok(new { status = true, message = "Cevaplar kaydedildi." });
        }
    }
}
