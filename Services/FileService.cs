using DoConnect.API.Data;
using DoConnect.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DoConnect.API.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadPath;

        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

        public FileService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _uploadPath = Path.Combine(_environment.ContentRootPath, "Uploads", "Images");

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<List<Image>> SaveFilesAsync(IFormFileCollection files, int? questionId = null, int? answerId = null)
        {
            var images = new List<Image>();

            foreach (var file in files)
            {
                if (file.Length == 0)
                    continue;

                if (file.Length > _maxFileSize)
                    throw new InvalidOperationException($"File {file.FileName} exceeds maximum size of 5MB");

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                    throw new InvalidOperationException($"File type {extension} is not allowed");

                // Sanitize and build filename
                var originalFileName = Path.GetFileName(file.FileName);
                var safeFileName = SanitizeFileName(originalFileName);

                var filePath = Path.Combine(_uploadPath, safeFileName);
                var finalFilePath = filePath;
                int counter = 1;

                // Add suffix if file exists to avoid overwrite
                while (File.Exists(finalFilePath))
                {
                    var nameWithoutExt = Path.GetFileNameWithoutExtension(safeFileName);
                    var ext = Path.GetExtension(safeFileName);
                    var newFileName = $"{nameWithoutExt}_{counter}{ext}";
                    finalFilePath = Path.Combine(_uploadPath, newFileName);
                    counter++;
                }

                // Save the file
                using (var stream = new FileStream(finalFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Save record with actual saved filename
                var image = new Image
                {
                    FileName = Path.GetFileName(finalFilePath), // ✅ This is correct
                    FilePath = finalFilePath,
                    ContentType = file.ContentType,
                    FileSize = file.Length,
                    QuestionId = questionId,
                    AnswerId = answerId,
                    UploadedAt = DateTime.UtcNow
                };

                _context.Images.Add(image);
                images.Add(image);
            }

            await _context.SaveChangesAsync();
            return images;
        }

        private string SanitizeFileName(string fileName)
        {
            // Replace invalid file name characters
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            
            // Replace spaces with underscores for better URL handling
            fileName = fileName.Replace(' ', '_');
            
            return fileName;
        }

        public async Task<Image?> GetImageAsync(int imageId)
        {
            return await _context.Images.FindAsync(imageId);
        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var image = await _context.Images.FindAsync(imageId);
            if (image == null)
                return false;

            if (File.Exists(image.FilePath))
            {
                File.Delete(image.FilePath);
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Stream> GetImageStreamAsync(int imageId)
        {
            var image = await GetImageAsync(imageId);
            if (image == null || !File.Exists(image.FilePath))
                throw new FileNotFoundException("Image not found");

            return new FileStream(image.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        // ✅ FIXED: This method should return the actual image URL
        public string GetImagePath(int imageId)
        {
            // This method might not be used if you're using static file serving
            // But if used, it should return the correct path
            return $"/api/images/{imageId}"; // or remove this method entirely
        }
    }
}
