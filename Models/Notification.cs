namespace DoConnect.API.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public int? RelatedQuestionId { get; set; }
        public int? RelatedAnswerId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public User User { get; set; } = null!;
        public Question? RelatedQuestion { get; set; }
        public Answer? RelatedAnswer { get; set; }
    }

    public enum NotificationType
    {
        QuestionAnswer = 1,
        QuestionVote = 2,
        AnswerVote = 3,
        QuestionApproved = 4,
        AnswerApproved = 5,
        NewFollower = 6,
        Achievement = 7
    }
}