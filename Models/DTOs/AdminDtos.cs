namespace DoConnect.API.Models.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalAnswers { get; set; }
        public int TotalImages { get; set; }
        public int PendingQuestions { get; set; }
        public int PendingAnswers { get; set; }
        public int ApprovedQuestions { get; set; }
        public int ApprovedAnswers { get; set; }
    }

    public class UpdateQuestionStatusDto
    {
        public int QuestionId { get; set; }
        public QuestionStatus Status { get; set; }
    }

    public class UpdateAnswerStatusDto
    {
        public int AnswerId { get; set; }
        public AnswerStatus Status { get; set; }
    }
}