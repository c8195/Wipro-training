using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DoConnect.API.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; } = new List<IdentityUserRole<int>>();
         public virtual UserProfile ?UserProfile { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        
        
    
}
        
    }

    public class Role : IdentityRole<int>
    {
        public Role() : base() { }
        public Role(string roleName) : base(roleName) { }
    }
