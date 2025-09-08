using DoConnect.API.Data;
using DoConnect.API.Models;
using DoConnect.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VotingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VotingController> _logger;

        public VotingController(ApplicationDbContext context, ILogger<VotingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Vote([FromBody] VoteRequestDto voteRequest)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                // Validate that either QuestionId or AnswerId is provided
                if (voteRequest.QuestionId == null && voteRequest.AnswerId == null)
                {
                    return BadRequest("Either QuestionId or AnswerId must be provided");
                }

                // Check for existing vote
                var existingVote = await _context.Votes
                    .FirstOrDefaultAsync(v => v.UserId == userId &&
                        v.QuestionId == voteRequest.QuestionId &&
                        v.AnswerId == voteRequest.AnswerId);

                if (existingVote != null)
                {
                    if (existingVote.Type == voteRequest.Type)
                    {
                        // Remove vote if same type
                        _context.Votes.Remove(existingVote);
                    }
                    else
                    {
                        // Update vote type
                        existingVote.Type = voteRequest.Type;
                    }
                }
                else
                {
                    // Create new vote
                    var newVote = new Vote
                    {
                        UserId = userId,
                        QuestionId = voteRequest.QuestionId,
                        AnswerId = voteRequest.AnswerId,
                        Type = voteRequest.Type,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Votes.Add(newVote);
                }

                await _context.SaveChangesAsync();

                // Return updated vote stats
                var voteStats = await GetVoteStatsAsync(voteRequest.QuestionId, voteRequest.AnswerId, userId);
                return Ok(voteStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing vote");
                return StatusCode(500, new { message = "An error occurred while processing your vote" });
            }
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetVoteStats(int? questionId, int? answerId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int.TryParse(userIdClaim, out int userId);

                var voteStats = await GetVoteStatsAsync(questionId, answerId, userId);
                return Ok(voteStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vote stats");
                return StatusCode(500, new { message = "An error occurred while getting vote statistics" });
            }
        }

        private async Task<VoteStatsDto> GetVoteStatsAsync(int? questionId, int? answerId, int userId)
        {
            var votes = await _context.Votes
                .Where(v => v.QuestionId == questionId && v.AnswerId == answerId)
                .ToListAsync();

            var upVotes = votes.Count(v => v.Type == VoteType.Upvote);
            var downVotes = votes.Count(v => v.Type == VoteType.Downvote);
            var userVote = votes.FirstOrDefault(v => v.UserId == userId);

            return new VoteStatsDto
            {
                UpVotes = upVotes,
                DownVotes = downVotes,
                NetVotes = upVotes - downVotes,
                UserVote = userVote?.Type
            };
        }
    }
}