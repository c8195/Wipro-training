using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoConnect.API.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public AnswerStatus Status { get; set; } = AnswerStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int UpVotes { get; set; } = 0;
        public int DownVotes { get; set; } = 0;

        // Foreign Keys
        public int QuestionId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    }

    public enum AnswerStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}