using AnketSistemi.API.DTOs;
using AnketSistemi.API.Enums; // QuestionFormat enum'ı için
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
    public class PollController : ControllerBase
    {
        private readonly IGenericRepository<Poll> _pollRepo;

        public PollController(IGenericRepository<Poll> pollRepo)
        {
            _pollRepo = pollRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? search = null)
        {
            var query = _pollRepo.AsQueryable().Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Title.ToLower().Contains(search.ToLower()));

            var polls = await query
                .Select(p => new PollViewDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    CreatedAt = p.CreatedAt,
                    QuestionCount = p.Questions!.Count
                }).ToListAsync();

            return Ok(polls);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var poll = await _pollRepo.AsQueryable()
                .Include(p => p.Questions!)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (poll == null) return NotFound(new { message = "Anket bulunamadı." });

            return Ok(poll);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(PollCreateDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var poll = new Poll
            {
                Title = model.Title,
                Detail = model.Detail,
                ExpireDate = model.ExpireDate,
                AppUserId = userId!,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            await _pollRepo.AddAsync(poll);
            await _pollRepo.SaveAsync();
            return Ok(new { status = true, message = "Anket başarıyla oluşturuldu." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestion(PollQuestionDto model, [FromServices] IGenericRepository<PollQuestion> questionRepo)
        {
            var question = new PollQuestion
            {
                PollId = model.PollId,
                QuestionText = model.QuestionText,
                Format = (QuestionFormat)model.Format,
                IsMandatory = model.IsMandatory,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            await questionRepo.AddAsync(question);
            await questionRepo.SaveAsync();
            return Ok(new { status = true, message = "Soru başarıyla eklendi." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddChoice")]
        public async Task<IActionResult> AddChoice(QuestionChoiceDto model, [FromServices] IGenericRepository<QuestionChoice> choiceRepo)
        {
            var choice = new QuestionChoice
            {
                PollQuestionId = model.PollQuestionId,
                ChoiceText = model.ChoiceText,
                OrderIndex = model.OrderIndex,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            await choiceRepo.AddAsync(choice);
            await choiceRepo.SaveAsync();
            return Ok(new { status = true, message = "Seçenek başarıyla eklendi." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(Poll model)
        {
            var poll = await _pollRepo.GetByIdAsync(model.Id);
            if (poll == null) return NotFound(new { message = "Güncellenecek anket bulunamadı." });

            poll.Title = model.Title;
            poll.Detail = model.Detail;
            poll.ExpireDate = model.ExpireDate;
            poll.IsActive = model.IsActive;

            _pollRepo.Update(poll);
            await _pollRepo.SaveAsync();
            return Ok(new { status = true, message = "Anket güncellendi." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var poll = await _pollRepo.GetByIdAsync(id);
            if (poll == null) return NotFound(new { message = "Silinecek anket bulunamadı." });

            poll.IsActive = false;
            _pollRepo.Update(poll);
            await _pollRepo.SaveAsync();
            return Ok(new { status = true, message = "Anket pasif hale getirildi." });
        }

        // GENEL VEYA ADMİN: Anket Sonuç İstatistikleri
        [HttpGet("{id}/Results")]
        public async Task<IActionResult> GetResults(int id, [FromServices] IGenericRepository<UserResponse> responseRepo)
        {
            var poll = await _pollRepo.AsQueryable()
                .Include(p => p.Questions!)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (poll == null) return NotFound(new { message = "Anket bulunamadı." });

            var allResponses = await responseRepo.AsQueryable()
                .Where(r => r.PollId == id)
                .ToListAsync();

            var results = poll.Questions!.Select(q => new
            {
                Question = q.QuestionText,
                TotalVotes = allResponses.Count(r => r.PollQuestionId == q.Id),
                Choices = q.Choices!.Select(c => new
                {
                    Text = c.ChoiceText,
                    VoteCount = allResponses.Count(r => r.SelectedChoiceId == c.Id)
                })
            });

            return Ok(new { pollTitle = poll.Title, statistics = results });
        }
    }
}