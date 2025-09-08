using DoConnect.API.Models;

namespace DoConnect.API.Services
{
    public interface IFileService
    {
        Task<List<Image>> SaveFilesAsync(IFormFileCollection files, int? questionId = null, int? answerId = null);
        Task<Image?> GetImageAsync(int imageId);
        Task<bool> DeleteImageAsync(int imageId);
        Task<Stream> GetImageStreamAsync(int imageId);
        string GetImagePath(int imageId);
    }
}