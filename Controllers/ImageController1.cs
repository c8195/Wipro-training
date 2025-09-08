// Controllers/ImagesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DoConnect.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(IWebHostEnvironment environment, ILogger<ImagesController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        [HttpGet("{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            try
            {
                var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads","Images");
                var filePath = Path.Combine(uploadsPath, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(filePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving image: {FileName}", fileName);
                return StatusCode(500);
            }
        }
    }
}