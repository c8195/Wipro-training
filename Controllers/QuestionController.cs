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
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(
            ApplicationDbContext context, 
            IMapper mapper, 
            IFileService fileService,
            ILogger<QuestionsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous] // Allow anonymous access to view questions
        public async Task<IActionResult> GetQuestions(
            [FromQuery] string? search,
            [FromQuery] string? topic,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("GetQuestions called with search: {Search}, topic: {Topic}", search, topic);

                var query = _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.Images)
                    .Include(q => q.Answers)
                    .Where(q => q.Status == QuestionStatus.Approved)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(q => q.Title.Contains(search) || q.Content.Contains(search));
                }

                if (!string.IsNullOrWhiteSpace(topic))
                {
                    query = query.Where(q => q.Topic == topic);
                }

                var totalCount = await query.CountAsync();
                var questions = await query
                    .OrderByDescending(q => q.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Manual mapping to avoid AutoMapper issues
                var questionDtos = questions.Select(q => new QuestionListDto
                {
                    Id = q.Id,
                    Title = q.Title ?? string.Empty,
                    Content = q.Content ?? string.Empty,
                    Topic = q.Topic ?? string.Empty,
                    Status = q.Status,
                    CreatedAt = q.CreatedAt,
                    AnswerCount = q.Answers?.Count ?? 0,
                    User = new UserDto
                    {
                        Id = q.User.Id,
                        FirstName = q.User.FirstName ?? string.Empty,
                        LastName = q.User.LastName ?? string.Empty,
                        Email = q.User.Email ?? string.Empty,
                        UserName = q.User.UserName ?? string.Empty
                    },
                    Images = q.Images?.Select(i => new ImageDto
                    {
                        Id = i.Id,
                        FileName = i.FileName ?? string.Empty,
                        ContentType = i.ContentType ?? string.Empty,
                        FileSize = i.FileSize,
                        UploadedAt = i.UploadedAt
                    }).ToList() ?? new List<ImageDto>()
                }).ToList();

                return Ok(new
                {
                    questions = questionDtos,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetQuestions");
                return StatusCode(500, new { message = "An error occurred while retrieving questions" });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Allow anonymous access to view individual questions
        public async Task<IActionResult> GetQuestion(int id)
        {
            try
            {
                _logger.LogInformation("GetQuestion called for ID: {Id}", id);

                var question = await _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.Images)
                    .Include(q => q.Answers.Where(a => a.Status == AnswerStatus.Approved))
                        .ThenInclude(a => a.User)
                    .Include(q => q.Answers.Where(a => a.Status == AnswerStatus.Approved))
                        .ThenInclude(a => a.Images)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (question == null)
                    return NotFound(new { message = "Question not found" });

                // Manual mapping
                var questionDto = new QuestionDto
                {
                    Id = question.Id,
                    Title = question.Title ?? string.Empty,
                    Content = question.Content ?? string.Empty,
                    Topic = question.Topic ?? string.Empty,
                    Status = question.Status,
                    CreatedAt = question.CreatedAt,
                    UpdatedAt = question.UpdatedAt,
                    AnswerCount = question.Answers?.Count ?? 0,
                    User = new UserDto
                    {
                        Id = question.User.Id,
                        FirstName = question.User.FirstName ?? string.Empty,
                        LastName = question.User.LastName ?? string.Empty,
                        Email = question.User.Email ?? string.Empty,
                        UserName = question.User.UserName ?? string.Empty
                    },
                    Images = question.Images.Select(i => new ImageDto
                    {
                        Id = i.Id,
                        FileName = i.FileName ?? string.Empty,
                        ContentType = i.ContentType ?? string.Empty,
                        FileSize = i.FileSize,
                        UploadedAt = i.UploadedAt,
                        Url = $"/api/images/{i.FileName}"
                    }).ToList() ?? new List<ImageDto>(),
                    
                    // ✅ NEW: Include all approved answers
                    Answers = question.Answers?
                        .Where(a => a.Status == AnswerStatus.Approved)
                        .OrderBy(a => a.CreatedAt)
                        .Select(a => new AnswerDto
                        {
                            Id = a.Id,
                            Content = a.Content ?? string.Empty,
                            Status = a.Status,
                            CreatedAt = a.CreatedAt,
                            UpdatedAt = a.UpdatedAt,
                            User = new UserDto
                            {
                                Id = a.User.Id,
                                FirstName = a.User.FirstName ?? string.Empty,
                                LastName = a.User.LastName ?? string.Empty,
                                Email = a.User.Email ?? string.Empty,
                                UserName = a.User.UserName ?? string.Empty
                            },
                            Images = a.Images?.Select(i => new ImageDto
                            {
                                Id = i.Id,
                                FileName = i.FileName ?? string.Empty,
                                ContentType = i.ContentType ?? string.Empty,
                                FileSize = i.FileSize,
                                UploadedAt = i.UploadedAt,
                                Url = $"/api/images/{i.FileName}"
                            }).ToList() ?? new List<ImageDto>()
                        }).ToList() ?? new List<AnswerDto>()
                };

                return Ok(questionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetQuestion for ID: {Id}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the question" });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateQuestion([FromForm] CreateQuestionDto? createQuestionDto, IFormFileCollection? files)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                _logger.LogInformation("=== CREATE QUESTION START ===");
                _logger.LogInformation("Received DTO - Title: {Title}, Content: {Content}, Topic: {Topic}", 
                    createQuestionDto?.Title, createQuestionDto?.Content, createQuestionDto?.Topic);

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage);
                    
                    _logger.LogWarning("Validation failed: {Errors}", string.Join(", ", errors));
                    return BadRequest(new { message = "Validation failed", errors });
                }

                // Get and validate user ID
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation("User ID claim: {UserIdClaim}", userIdClaim);
                
                if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    _logger.LogError("Invalid user ID claim: {UserIdClaim}", userIdClaim);
                    return Unauthorized(new { message = "Invalid user token" });
                }

                // Verify user exists in database
                var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                _logger.LogInformation("User exists in database: {UserExists} for ID: {UserId}", userExists, userId);
                
                if (!userExists)
                {
                    _logger.LogError("User not found in database: {UserId}", userId);
                    return BadRequest(new { message = "User not found" });
                }

                // Check database connection
                var canConnect = await _context.Database.CanConnectAsync();
                _logger.LogInformation("Database can connect: {CanConnect}", canConnect);
                
                if (!canConnect)
                {
                    _logger.LogError("Cannot connect to database");
                    return StatusCode(500, new { message = "Database connection failed" });
                }

                // Count existing questions before insertion
                var existingCount = await _context.Questions.CountAsync();
                _logger.LogInformation("Existing questions count before insert: {Count}", existingCount);

                // Create question entity
                var question = new Question
                {
                    Title = createQuestionDto!.Title?.Trim() ??string.Empty, 
                    Content = createQuestionDto.Content?.Trim() ?? string.Empty,
                    Topic = createQuestionDto.Topic?.Trim() ?? string.Empty,
                    UserId = userId,
                    Status = QuestionStatus.Pending, // Set to Approved for immediate visibility
                    CreatedAt = DateTime.UtcNow
                };

                _logger.LogInformation("Created question entity - Title: {Title}, UserId: {UserId}, Status: {Status}", 
                    question.Title, question.UserId, question.Status);

                // Add to context
                _context.Questions.Add(question);
                _logger.LogInformation("Added question to context");

                // Check pending changes
                var pendingChanges = _context.ChangeTracker.Entries().Count(e => e.State != EntityState.Unchanged);
                _logger.LogInformation("Pending changes in context: {PendingChanges}", pendingChanges);

                // Save changes with detailed error handling
                _logger.LogInformation("Calling SaveChangesAsync...");
                
                var saveResult = 0;
                try
                {
                    saveResult = await _context.SaveChangesAsync();
                    _logger.LogInformation("SaveChangesAsync completed. Rows affected: {SaveResult}", saveResult);
                }
                catch (Exception saveEx)
                {
                    _logger.LogError(saveEx, "SaveChangesAsync failed: {Message}", saveEx.Message);
                    _logger.LogError("Inner exception: {InnerException}", saveEx.InnerException?.Message);
                    await transaction.RollbackAsync();
                    return StatusCode(500, new { 
                        message = "Failed to save question", 
                        error = saveEx.Message,
                        innerError = saveEx.InnerException?.Message
                    });
                }

                if (saveResult == 0)
                {
                    _logger.LogError("SaveChanges returned 0 - no rows affected");
                    await transaction.RollbackAsync();
                    return StatusCode(500, new { message = "No rows were saved to database" });
                }

                // Verify question was actually saved
                var savedQuestion = await _context.Questions.FindAsync(question.Id);
                if (savedQuestion == null)
                {
                    _logger.LogError("Question not found after save - ID: {QuestionId}", question.Id);
                    await transaction.RollbackAsync();
                    return StatusCode(500, new { message = "Question was not properly saved" });
                }

                _logger.LogInformation("Question verified in database - ID: {QuestionId}", savedQuestion.Id);

                // Count questions after insertion
                var newCount = await _context.Questions.CountAsync();
                _logger.LogInformation("Questions count after insert: {Count} (was {OldCount})", newCount, existingCount);

                // Handle file uploads
                if (files != null && files.Count > 0)
                {
                    try
                    {
                        _logger.LogInformation("Processing {FileCount} files", files.Count);
                        await _fileService.SaveFilesAsync(files, questionId: question.Id);
                        _logger.LogInformation("Files saved successfully");
                    }
                    catch (Exception fileEx)
                    {
                        _logger.LogError(fileEx, "File upload failed: {Message}", fileEx.Message);
                        await transaction.RollbackAsync();
                        return BadRequest(new { message = $"File upload failed: {fileEx.Message}" });
                    }
                }

                // Commit transaction
                await transaction.CommitAsync();
                _logger.LogInformation("Transaction committed successfully");

                // Reload with includes
                var createdQuestion = await _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.Images)
                    .FirstOrDefaultAsync(q => q.Id == question.Id);

                if (createdQuestion == null)
                {
                    _logger.LogError("Could not reload created question");
                    return StatusCode(500, new { message = "Question created but could not be retrieved" });
                }

                // Create response DTO
                var questionDto = new QuestionDto
                {
                    Id = createdQuestion.Id,
                    Title = createdQuestion.Title ?? string.Empty,
                    Content = createdQuestion.Content ?? string.Empty,
                    Topic = createdQuestion.Topic ?? string.Empty,
                    Status = createdQuestion.Status,
                    CreatedAt = createdQuestion.CreatedAt,
                    UpdatedAt = createdQuestion.UpdatedAt,
                    AnswerCount = 0,
                    User = new UserDto
                    {
                        Id = createdQuestion.User.Id,
                        FirstName = createdQuestion.User.FirstName ?? string.Empty,
                        LastName = createdQuestion.User.LastName ?? string.Empty,
                        Email = createdQuestion.User.Email ?? string.Empty,
                        UserName = createdQuestion.User.UserName ?? string.Empty
                    },
                    Images = createdQuestion.Images?.Select(i => new ImageDto
                    {
                        Id = i.Id,
                        FileName = i.FileName ?? string.Empty,
                        ContentType = i.ContentType ?? string.Empty,
                        FileSize = i.FileSize,
                        UploadedAt = i.UploadedAt
                    }).ToList() ?? new List<ImageDto>(),
                    Answers = new List<AnswerDto>() // New question has no answers initially
                };

                _logger.LogInformation("=== CREATE QUESTION SUCCESS - ID: {QuestionId} ===", question.Id);

                return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, questionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "=== CREATE QUESTION FAILED ===");
                _logger.LogError("Exception details: {Message}", ex.Message);
                _logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
                
                await transaction.RollbackAsync();
                
                return StatusCode(500, new { 
                    message = "An error occurred while creating the question",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpGet("topics")]
        [AllowAnonymous] // Allow anonymous access to topics
        public async Task<IActionResult> GetTopics()
        {
            try
            {
                _logger.LogInformation("GetTopics called");

                var topics = await _context.Questions
                    .Where(q => q.Status == QuestionStatus.Approved)
                    .GroupBy(q => q.Topic)
                    .Select(g => new TopicDto
                    {
                        Topic = g.Key ?? string.Empty,
                        Count = g.Count()
                    })
                    .OrderByDescending(t => t.Count)
                    .Take(20)
                    .ToListAsync();

                return Ok(topics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTopics");
                return StatusCode(500, new { message = "An error occurred while retrieving topics" });
            }
        }

        [HttpGet("my")]
        [Authorize] // Require authentication for user's own questions
        public async Task<IActionResult> GetMyQuestions()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "Invalid user token" });
                }

                _logger.LogInformation("GetMyQuestions called for user: {UserId}", userId);

                var questions = await _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.Images)
                    .Include(q => q.Answers)
                    .Where(q => q.UserId == userId)
                    .OrderByDescending(q => q.CreatedAt)
                    .ToListAsync();

                // Manual mapping
                var questionDtos = questions.Select(q => new QuestionListDto
                {
                    Id = q.Id,
                    Title = q.Title ?? string.Empty,
                    Content = q.Content ?? string.Empty,
                    Topic = q.Topic ?? string.Empty,
                    Status = q.Status,
                    CreatedAt = q.CreatedAt,
                    AnswerCount = q.Answers?.Count ?? 0,
                    User = new UserDto
                    {
                        Id = q.User.Id,
                        FirstName = q.User.FirstName ?? string.Empty,
                        LastName = q.User.LastName ?? string.Empty,
                        Email = q.User.Email ?? string.Empty,
                        UserName = q.User.UserName ?? string.Empty
                    },
                    Images = q.Images?.Select(i => new ImageDto
                    {
                        Id = i.Id,
                        FileName = i.FileName ?? string.Empty,
                        ContentType = i.ContentType ?? string.Empty,
                        FileSize = i.FileSize,
                        UploadedAt = i.UploadedAt
                    }).ToList() ?? new List<ImageDto>()
                }).ToList();

                return Ok(questionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMyQuestions");
                return StatusCode(500, new { message = "An error occurred while retrieving your questions" });
            }
        }
    }
}
