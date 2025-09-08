namespace DoConnect.API.Models.DTOs
{
    public class VoteRequestDto
    {
        public int? QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public VoteType Type { get; set; }
    }

    public class VoteStatsDto
    {
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public int NetVotes { get; set; }
        public VoteType? UserVote { get; set; }
    }
}