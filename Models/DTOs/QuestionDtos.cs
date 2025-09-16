using System.ComponentModel.DataAnnotations;

namespace DoConnect.API.Models.DTOs
{
    public class CreateQuestionDto
    {

        [StringLength(200, MinimumLength = 10)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(20)]
        public string Content { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Topic { get; set; } = string.Empty;
        

        public List<string> Tags { get; set; } = new List<string>();
    }

    public class UpdateQuestionDto
    {
        [Required]
        [StringLength(200, MinimumLength = 10)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(20)]
        public string Content { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Topic { get; set; } = string.Empty;
    }

    public class QuestionDto
    {
        public int Id { get; set; }
       public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public QuestionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserDto User { get; set; } = null!;
        public List<ImageDto> Images { get; set; } = new List<ImageDto>();
        public int AnswerCount { get; set; }
         public VoteStatsDto VoteStats { get; set; } = new VoteStatsDto();
    public List<string> Tags { get; set; } = new List<string>();
    public int ViewCount { get; set; }
    public bool IsBookmarked { get; set; }
    public DateTime? LastActivity { get; set; }
    }

    public class QuestionListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public QuestionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto User { get; set; } = null!;
        public int AnswerCount { get; set; }
        public List<ImageDto> Images { get; set; } = new List<ImageDto>();
    }

    public class TopicDto
    {
        public string Topic { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}