namespace DoConnect.API.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Bio { get; set; }
        public string? Website { get; set; }
        public string? Location { get; set; }
        public string? ProfilePicture { get; set; }
        public int Reputation { get; set; } = 0;
        public DateTime JoinedAt { get; set; }
        public DateTime? LastSeenAt { get; set; }
        
        // Navigation properties
        public  virtual User User { get; set; }  = null!;
    }
}