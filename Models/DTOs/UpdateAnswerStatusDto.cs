namespace DoConnect.API.Models.DTOs.Admin
{
    public class UpdateAnswerStatusDto
    {
        public int Status { get; set; } // 0=Pending, 1=Approved, 2=Rejected
    }
}
