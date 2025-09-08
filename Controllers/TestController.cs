using Microsoft.AspNetCore.Mvc;

namespace DoConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Ping endpoint called");
            return Ok(new { message = "Pong", timestamp = DateTime.UtcNow });
        }

        [HttpPost("test-register")]
        public IActionResult TestRegister([FromBody] object data)
        {
            try
            {
                _logger.LogInformation("Test register called with data: {Data}", data?.ToString());
                return Ok(new { message = "Test register successful", data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in test register");
                return BadRequest(new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}