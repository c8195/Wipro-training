using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoConnect.API.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys (nullable - can belong to question or answer)
        public int? QuestionId { get; set; }
        public int? AnswerId { get; set; }

        // Navigation properties
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }
        [ForeignKey("AnswerId")]
        public virtual Answer? Answer { get; set; }
    }
}