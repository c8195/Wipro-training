using System.ComponentModel.DataAnnotations;

namespace DoConnect.API.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public VoteType Type { get; set; } // Upvote or Downvote
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public User User { get; set; } = null!;
        public Question? Question { get; set; }
        public Answer? Answer { get; set; }
    }

    public enum VoteType
    {
        Downvote = -1,
        Upvote = 1
    }
}