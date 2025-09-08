using System.ComponentModel.DataAnnotations;

namespace DoConnect.API.Models.DTOs
{
    public class CreateAnswerDto
    {
        [Required]
        [MinLength(10)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int QuestionId { get; set; }
    }

    public class UpdateAnswerDto
    {
        [Required]
        [MinLength(10)]
        public string Content { get; set; } = string.Empty;
    }

    public class AnswerDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public AnswerStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int QuestionId { get; set; }
        public UserDto User { get; set; } = null!;
        public List<ImageDto> Images { get; set; } = new List<ImageDto>();
    }

    public class ImageDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}