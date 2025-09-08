namespace DoConnect.API.Models.DTOs.Admin
{
    public class UpdateQuestionStatusDto
    {
        public int Status { get; set; } // 0=Pending, 1=Approved, 2=Rejected
    }
}
