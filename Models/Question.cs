using System.ComponentModel.DataAnnotations;


using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace DoConnect.API.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Topic { get; set; } = string.Empty;


        public int UpVotes { get; set; } = 0;
        public int DownVotes { get; set; } = 0;
        public int ViewCount { get; set; } = 0;
        public DateTime? LastActivity { get; set; }

        public QuestionStatus Status { get; set; } = QuestionStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Foreign Keys
        public int UserId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<Image> Images { get; set; } = new List<Image>();

        // Computed property
        [NotMapped]
        public int AnswerCount => Answers?.Count ?? 0;
    }

    public enum QuestionStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
    
}


