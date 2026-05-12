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

        [HttpPost("Submit")]
        public async Task<IActionResult> Submit(int pollId, List<QuestionAnswerDto> answers, [FromServices] IGenericRepository<Poll> pollRepo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var poll = await pollRepo.GetByIdAsync(pollId);
            if (poll == null || !poll.IsActive)
                return NotFound(new { message = "Anket bulunamadı veya pasif." });

            if (poll.ExpireDate < DateTime.Now)
                return BadRequest(new { status = false, message = "Bu anketin süresi dolmuştur, artık cevap verilemez." });

            var alreadyAnswered = await _responseRepo.AsQueryable()
                .AnyAsync(r => r.PollId == pollId && r.AppUserId == userId);

            if (alreadyAnswered)
                return BadRequest(new { status = false, message = "Bu anketi zaten tamamladınız." });


            foreach (var item in answers)
            {
                var response = new UserResponse
                {
                    PollId = pollId,
                    PollQuestionId = item.QuestionId,
                    WrittenAnswer = item.TextAnswer,
                    SelectedChoiceId = item.SelectedOptionId,
                    AppUserId = userId!,
                    CreatedAt = DateTime.Now
                };
                await _responseRepo.AddAsync(response);
            }

            await _responseRepo.SaveAsync();
            return Ok(new { status = true, message = "Cevaplarınız kaydedildi." });
        }
    }
}