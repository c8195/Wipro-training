using DoConnect.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                var image = await _fileService.GetImageAsync(id);
                if (image == null)
                    return NotFound();

                var stream = await _fileService.GetImageStreamAsync(id);
                return File(stream, image.ContentType, image.FileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var result = await _fileService.DeleteImageAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}