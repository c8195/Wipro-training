using DoConnect.API.Data;
using DoConnect.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DebugController> _logger;

        public DebugController(ApplicationDbContext context, ILogger<DebugController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("questions-raw")]
        public async Task<IActionResult> GetAllQuestionsRaw()
        {
            try
            {
                _logger.LogInformation("Fetching all questions from database");

                var questions = await _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.Images)
                    .Include(q => q.Answers)
                    .Select(q => new
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Content = q.Content,
                        Topic = q.Topic,
                        Status = q.Status,
                        StatusText = q.Status.ToString(),
                        CreatedAt = q.CreatedAt,
                        UpdatedAt = q.UpdatedAt,
                        UserId = q.UserId,
                        User = new
                        {
                            Id = q.User.Id,
                            FirstName = q.User.FirstName,
                            LastName = q.User.LastName,
                            Email = q.User.Email,
                            UserName = q.User.UserName
                        },
                        ImageCount = q.Images.Count,
                        AnswerCount = q.Answers.Count,
                        Images = q.Images.Select(i => new
                        {
                            Id = i.Id,
                            FileName = i.FileName,
                            ContentType = i.ContentType,
                            FileSize = i.FileSize,
                            UploadedAt = i.UploadedAt
                        }).ToList()
                    })
                    .OrderByDescending(q => q.CreatedAt)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} questions from database", questions.Count);

                return Ok(new
                {
                    TotalQuestions = questions.Count,
                    DatabaseName = _context.Database.GetDbConnection().Database,
                    Questions = questions
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving questions");
                return StatusCode(500, new { 
                    message = "Error retrieving questions", 
                    details = ex.Message,
                    stackTrace = ex.StackTrace 
                });
            }
        }

        [HttpGet("questions-by-status")]
        public async Task<IActionResult> GetQuestionsByStatus()
        {
            try
            {
                var statusCounts = await _context.Questions
                    .GroupBy(q => q.Status)
                    .Select(g => new
                    {
                        Status = (int)g.Key,
                        StatusName = g.Key.ToString(),
                        Count = g.Count()
                    })
                    .ToListAsync();

                var totalQuestions = await _context.Questions.CountAsync();

                return Ok(new
                {
                    TotalQuestions = totalQuestions,
                    StatusBreakdown = statusCounts,
                    DatabaseName = _context.Database.GetDbConnection().Database
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting questions by status");
                return StatusCode(500, new { message = "Error getting questions by status", details = ex.Message });
            }
        }

        [HttpGet("database-info")]
        public async Task<IActionResult> GetDatabaseInfo()
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();
                var dbName = _context.Database.GetDbConnection().Database;
                var canConnect = await _context.Database.CanConnectAsync();
                
                // Count all tables
                var userCount = await _context.Users.CountAsync();
                var questionCount = await _context.Questions.CountAsync();
                var answerCount = await _context.Answers.CountAsync();
                var imageCount = await _context.Images.CountAsync();

                return Ok(new
                {
                    ConnectionString = connectionString?.Replace("Password=", "Password=****"),
                    DatabaseName = dbName,
                    ServerName = _context.Database.GetDbConnection().DataSource,
                    CanConnect = canConnect,
                    TableCounts = new
                    {
                        Users = userCount,
                        Questions = questionCount,
                        Answers = answerCount,
                        Images = imageCount
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting database info");
                return StatusCode(500, new { message = "Error getting database info", details = ex.Message });
            }
        }

        [HttpGet("test-sql-query")]
        public async Task<IActionResult> TestSqlQuery()
        {
            try
            {
                // Raw SQL query to verify data exists
                var questions = await _context.Questions
                    .FromSqlRaw("SELECT * FROM Questions ORDER BY CreatedAt DESC")
                    .Select(q => new
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Topic = q.Topic,
                        Status = q.Status,
                        CreatedAt = q.CreatedAt,
                        UserId = q.UserId
                    })
                    .ToListAsync();

                return Ok(new
                {
                    DatabaseName = _context.Database.GetDbConnection().Database,
                    QuestionsFromRawSQL = questions,
                    Count = questions.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing raw SQL query");
                return StatusCode(500, new { 
                    message = "Error executing SQL query", 
                    details = ex.Message 
                });
            }
        }

        [HttpGet("connection-test")]
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                _logger.LogInformation("Testing database connection...");

                var connection = _context.Database.GetDbConnection();
                var connectionInfo = new
                {
                    ConnectionString = connection.ConnectionString?.Replace("Password=", "Password=****"),
                    Database = connection.Database,
                    DataSource = connection.DataSource,
                    ServerVersion = connection.ServerVersion,
                    State = connection.State.ToString()
                };

                // Test opening connection
                await connection.OpenAsync();
                var isOpen = connection.State == System.Data.ConnectionState.Open;
                await connection.CloseAsync();

                // Test executing a simple query
                var serverTime = await _context.Database.SqlQueryRaw<DateTime>("SELECT GETDATE()").FirstAsync();

                return Ok(new
                {
                    ConnectionInfo = connectionInfo,
                    CanOpenConnection = isOpen,
                    ServerTime = serverTime,
                    LocalTime = DateTime.Now,
                    UTCTime = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection test failed");
                return StatusCode(500, new 
                { 
                    message = "Database connection test failed", 
                    details = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}