// ApproveQuestionDto.cs
namespace DoConnect.API.Models.DTOs
{
    public class ApproveQuestionDto
    {
        public int QuestionId { get; set; }
        public bool IsApproved { get; set; }
        public string ?rejectionReason { get; set; }
    }
}