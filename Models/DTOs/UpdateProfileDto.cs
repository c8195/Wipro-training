namespace DoConnect.API.Models.DTOs
{
    public class UpdateProfileDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Bio { get; set; } = "";
        public string Website { get; set; } = "";
        public string Location { get; set; } = "";
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }
}
