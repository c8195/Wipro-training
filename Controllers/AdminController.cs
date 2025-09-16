using DoConnect.API.Data;
using DoConnect.API.Models;
using DoConnect.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DoConnect.API.Services;

namespace DoConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AdminController> _logger;
        private readonly INotificationService _notificationService;

        public AdminController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<AdminController> logger,
            INotificationService notificationService
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _notificationService = notificationService;
        }

        #region Dashboard Stats
        [HttpGet("dashboard/stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var stats = new
                {
                    TotalUsers = await _context.Users.CountAsync(),
                    TotalQuestions = await _context.Questions.CountAsync(),
                    TotalAnswers = await _context.Answers.CountAsync(),
                    TotalImages = await _context.Images.CountAsync(),
                    PendingQuestions = await _context.Questions.CountAsync(q => q.Status == QuestionStatus.Pending),
                    PendingAnswers = await _context.Answers.CountAsync(a => a.Status == AnswerStatus.Pending),
                    ApprovedQuestions = await _context.Questions.CountAsync(q => q.Status == QuestionStatus.Approved),
                    ApprovedAnswers = await _context.Answers.CountAsync(a => a.Status == AnswerStatus.Approved),
                    RejectedQuestions = await _context.Questions.CountAsync(q => q.Status == QuestionStatus.Rejected),
                    RejectedAnswers = await _context.Answers.CountAsync(a => a.Status == AnswerStatus.Rejected),
                    ActiveUsersToday = await _context.Users.CountAsync(u => u.IsActive)
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard stats");
                return StatusCode(500, new { message = "Error retrieving dashboard statistics" });
            }
        }
        #endregion

        #region Questions Management
        [HttpGet("questions/pending")]
        public async Task<IActionResult> GetPendingQuestions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.Images)
                    .Where(q => q.Status == QuestionStatus.Pending);

                var totalCount = await query.CountAsync();
                var questions = await query
                    .OrderByDescending(q => q.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(q => new
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Content = q.Content,
                        Topic = q.Topic,
                        Status = q.Status,
                        CreatedAt = q.CreatedAt,
                        User = new
                        {
                            Id = q.User.Id,
                            FirstName = q.User.FirstName,
                            LastName = q.User.LastName,
                            Email = q.User.Email,
                            UserName = q.User.UserName
                        },
                        Images = q.Images.Select(i => new
                        {
                            Id = i.Id,
                            FileName = i.FileName,
                            ContentType = i.ContentType,
                            FileSize = i.FileSize,
                            UploadedAt = i.UploadedAt
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(new
                {
                    questions,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending questions");
                return StatusCode(500, new { message = "Error retrieving pending questions" });
            }
        }

        [HttpPut("questions/{id}/status")]
        public async Task<IActionResult> UpdateQuestionStatus(int id, [FromBody] UpdateQuestionStatusDto statusDto)
        {
            try
            {
                var question = await _context.Questions
                    .Include(q => q.User)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (question == null)
                    return NotFound(new { message = "Question not found" });

                question.Status = (QuestionStatus)statusDto.Status;
                question.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // ✅ Notification fix
                if (question.Status == QuestionStatus.Approved)
                {
                    await _notificationService.NotifyUserAsync(
                        question.UserId,
                        "Question Approved",
                        $"Your question '{question.Title}' has been approved and is now visible to everyone."
                    );
                }
                else if (question.Status == QuestionStatus.Rejected)
                {
                    await _notificationService.NotifyUserAsync(
                        question.UserId,
                        "Question Rejected",
                        $"Your question '{question.Title}' has been rejected by the admin."
                    );
                }

                var statusText = question.Status == QuestionStatus.Approved ? "approved" : "rejected";
                return Ok(new { message = $"Question {statusText} successfully", status = question.Status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question status for ID: {QuestionId}", id);
                return StatusCode(500, new { message = "Error updating question status" });
            }
        }

        [HttpDelete("questions/{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                var question = await _context.Questions
                    .Include(q => q.Images)
                    .Include(q => q.Answers)
                        .ThenInclude(a => a.Images)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (question == null)
                    return NotFound(new { message = "Question not found" });

                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Question deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting question ID: {QuestionId}", id);
                return StatusCode(500, new { message = "Error deleting question" });
            }
        }


        [HttpGet("questions")]
        public async Task<IActionResult> GetAllQuestions([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? status = null)
        {
            try
            {
                var query = _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.Images)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(status) && Enum.TryParse<QuestionStatus>(status, true, out var questionStatus))
                {
                    query = query.Where(q => q.Status == questionStatus);
                }

                var totalCount = await query.CountAsync();
                var questions = await query
                    .OrderByDescending(q => q.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(q => new
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Content = q.Content,
                        Topic = q.Topic,
                        Status = q.Status,
                        CreatedAt = q.CreatedAt,
                        UpdatedAt = q.UpdatedAt,
                        User = new
                        {
                            Id = q.User.Id,
                            FirstName = q.User.FirstName,
                            LastName = q.User.LastName,
                            Email = q.User.Email
                        },
                        AnswerCount = q.Answers.Count
                    })
                    .ToListAsync();

                return Ok(new
                {
                    questions,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all questions");
                return StatusCode(500, new { message = "Error retrieving questions" });
            }
        }

        #endregion

        #region Answers Management
        [HttpGet("answers/pending")]
        public async Task<IActionResult> GetPendingAnswers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = _context.Answers
                    .Include(a => a.User)
                    .Include(a => a.Question)
                    .Include(a => a.Images)
                    .Where(a => a.Status == AnswerStatus.Pending);

                var totalCount = await query.CountAsync();
                var answers = await query
                    .OrderByDescending(a => a.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new
                    {
                        Id = a.Id,
                        Content = a.Content,
                        Status = a.Status,
                        CreatedAt = a.CreatedAt,
                        QuestionId = a.QuestionId,
                        QuestionTitle = a.Question.Title,
                        User = new
                        {
                            Id = a.User.Id,
                            FirstName = a.User.FirstName,
                            LastName = a.User.LastName,
                            Email = a.User.Email,
                            UserName = a.User.UserName
                        },
                        Images = a.Images.Select(i => new
                        {
                            Id = i.Id,
                            FileName = i.FileName,
                            ContentType = i.ContentType,
                            FileSize = i.FileSize,
                            UploadedAt = i.UploadedAt
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(new
                {
                    answers,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending answers");
                return StatusCode(500, new { message = "Error retrieving pending answers" });
            }
        }

        [HttpPut("answers/{id}/status")]
        public async Task<IActionResult> UpdateAnswerStatus(int id, [FromBody] UpdateAnswerStatusDto statusDto)
        {
            try
            {
                var answer = await _context.Answers.FindAsync(id);
                if (answer == null)
                    return NotFound(new { message = "Answer not found" });

                answer.Status = (AnswerStatus)statusDto.Status;
                answer.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var statusText = answer.Status == AnswerStatus.Approved ? "approved" : "rejected";
                return Ok(new { message = $"Answer {statusText} successfully", status = answer.Status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating answer status for ID: {AnswerId}", id);
                return StatusCode(500, new { message = "Error updating answer status" });
            }
        }

        [HttpDelete("answers/{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            try
            {
                var answer = await _context.Answers
                    .Include(a => a.Images)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (answer == null)
                    return NotFound(new { message = "Answer not found" });

                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Answer deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting answer ID: {AnswerId}", id);
                return StatusCode(500, new { message = "Error deleting answer" });
            }
        }

        [HttpGet("answers")]
        public async Task<IActionResult> GetAllAnswers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? status = null)
        {
            try
            {
                var query = _context.Answers
                    .Include(a => a.User)
                    .Include(a => a.Question)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(status) && Enum.TryParse<AnswerStatus>(status, true, out var answerStatus))
                {
                    query = query.Where(a => a.Status == answerStatus);
                }

                var totalCount = await query.CountAsync();
                var answers = await query
                    .OrderByDescending(a => a.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new
                    {
                        Id = a.Id,
                        Content = a.Content,
                        Status = a.Status,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt,
                        QuestionTitle = a.Question.Title,
                        User = new
                        {
                            Id = a.User.Id,
                            FirstName = a.User.FirstName,
                            LastName = a.User.LastName,
                            Email = a.User.Email
                        }
                    })
                    .ToListAsync();

                return Ok(new
                {
                    answers,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all answers");
                return StatusCode(500, new { message = "Error retrieving answers" });
            }
        }
        #endregion

        #region Users Management
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] bool? isActive = null)
        {
            try
            {
                var query = _context.Users.AsQueryable();

                if (isActive.HasValue)
                {
                    query = query.Where(u => u.IsActive == isActive.Value);
                }

                var totalCount = await query.CountAsync();
                var users = await query
                    .OrderByDescending(u => u.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDtos = new List<object>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var questionsCount = await _context.Questions.CountAsync(q => q.UserId == user.Id);
                    var answersCount = await _context.Answers.CountAsync(a => a.UserId == user.Id);

                    userDtos.Add(new
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        UserName = user.UserName,
                        IsActive = user.IsActive,
                        EmailConfirmed = user.EmailConfirmed,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt,
                        Roles = roles.ToList(),
                        QuestionsCount = questionsCount,
                        AnswersCount = answersCount
                    });
                }

                return Ok(new
                {
                    users = userDtos,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, new { message = "Error retrieving users" });
            }
        }

        [HttpPut("users/{id}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                    return NotFound(new { message = "User not found" });

                // Don't allow deactivating the current admin user
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (user.Id == currentUserId)
                    return BadRequest(new { message = "Cannot deactivate your own account" });

                user.IsActive = !user.IsActive;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var statusText = user.IsActive ? "activated" : "deactivated";
                    return Ok(new { 
                        message = $"User {statusText} successfully",
                        isActive = user.IsActive
                    });
                }

                return BadRequest(new { message = "Failed to update user status" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling user status for ID: {UserId}", id);
                return StatusCode(500, new { message = "Error updating user status" });
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                    return NotFound(new { message = "User not found" });

                // Don't allow deleting the current admin user
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (user.Id == currentUserId)
                    return BadRequest(new { message = "Cannot delete your own account" });

                // Don't allow deleting other admin users
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Contains("Admin"))
                    return BadRequest(new { message = "Cannot delete admin users" });

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new { message = "User deleted successfully" });
                }

                return BadRequest(new { message = "Failed to delete user", errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user ID: {UserId}", id);
                return StatusCode(500, new { message = "Error deleting user" });
            }
        }

        [HttpPost("users/{id}/roles")]
        public async Task<IActionResult> UpdateUserRoles(int id, [FromBody] UpdateUserRolesDto rolesDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                    return NotFound(new { message = "User not found" });

                var currentRoles = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                
                if (!removeResult.Succeeded)
                    return BadRequest(new { message = "Failed to remove current roles" });

                var addResult = await _userManager.AddToRolesAsync(user, rolesDto.Roles);
                if (addResult.Succeeded)
                {
                    return Ok(new { message = "User roles updated successfully" });
                }

                return BadRequest(new { message = "Failed to add new roles", errors = addResult.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user roles for ID: {UserId}", id);
                return StatusCode(500, new { message = "Error updating user roles" });
            }
        }
        #endregion













    }
}




