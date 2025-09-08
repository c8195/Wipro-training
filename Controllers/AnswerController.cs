using AutoMapper;
using DoConnect.API.Data;
using DoConnect.API.Models;
using DoConnect.API.Models.DTOs;
using DoConnect.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnswersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public AnswersController(ApplicationDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpGet("question/{questionId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAnswersForQuestion(int questionId)
        {
            var answers = await _context.Answers
                .Include(a => a.User)
                .Include(a => a.Images)
                .Where(a => a.QuestionId == questionId && a.Status == AnswerStatus.Approved)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            var answerDtos = _mapper.Map<List<AnswerDto>>(answers);
            return Ok(answerDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnswer([FromForm] CreateAnswerDto createAnswerDto, IFormFileCollection? files)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            // Check if question exists
            var questionExists = await _context.Questions.AnyAsync(q => q.Id == createAnswerDto.QuestionId);
            if (!questionExists)
                return BadRequest(new { message = "Question not found" });

            var answer = new Answer
            {
                Content = createAnswerDto.Content,
                QuestionId = createAnswerDto.QuestionId,
                UserId = userId,
                Status = AnswerStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            // Handle file uploads
            if (files != null && files.Count > 0)
            {
                try
                {
                    await _fileService.SaveFilesAsync(files, answerId: answer.Id);
                }
                catch (InvalidOperationException ex)
                {
                    _context.Answers.Remove(answer);
                    await _context.SaveChangesAsync();
                    return BadRequest(new { message = ex.Message });
                }
            }

            // Reload answer with includes
            var createdAnswer = await _context.Answers
                .Include(a => a.User)
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == answer.Id);

            var answerDto = _mapper.Map<AnswerDto>(createdAnswer);
            return CreatedAtAction(nameof(GetAnswersForQuestion), 
                new { questionId = answer.QuestionId }, answerDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswer(int id, [FromForm] UpdateAnswerDto updateAnswerDto, IFormFileCollection? files)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var answer = await _context.Answers
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (answer == null)
                return NotFound(new { message = "Answer not found" });

            if (answer.UserId != userId)
                return Forbid("You can only edit your own answers");

            answer.Content = updateAnswerDto.Content;
            answer.UpdatedAt = DateTime.UtcNow;
            answer.Status = AnswerStatus.Pending; // Reset status for re-moderation

            // Handle new file uploads
            if (files != null && files.Count > 0)
            {
                await _fileService.SaveFilesAsync(files, answerId: answer.Id);
            }

            await _context.SaveChangesAsync();

            // Reload answer with includes
            var updatedAnswer = await _context.Answers
                .Include(a => a.User)
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id);

            var answerDto = _mapper.Map<AnswerDto>(updatedAnswer);
            return Ok(answerDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var answer = await _context.Answers
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (answer == null)
                return NotFound(new { message = "Answer not found" });

            if (answer.UserId != userId)
                return Forbid("You can only delete your own answers");

            // Delete associated images
            foreach (var image in answer.Images)
            {
                await _fileService.DeleteImageAsync(image.Id);
            }

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyAnswers()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var answers = await _context.Answers
                .Include(a => a.User)
                .Include(a => a.Images)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            var answerDtos = _mapper.Map<List<AnswerDto>>(answers);
            return Ok(answerDtos);
        }
    }
}