// src/DoConnect.API/Controllers/ProfileController.cs

using DoConnect.API.Data;
using DoConnect.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DoConnect.API.Models;

namespace DoConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
 
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<ProfileController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                // Try both common claim types
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

                if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                    return Unauthorized(new { message = "Invalid user token" });

                // Fetch user and profile separately
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    return NotFound(new { message = "User not found" });

                // Profile may not exist yet
                var profile = await _context.UserProfiles
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                // Get counts
                var questionsCount = await _context.Questions.CountAsync(q => q.UserId == userId && q.Status == QuestionStatus.Approved);
                var answersCount = await _context.Answers.CountAsync(a => a.UserId == userId && a.Status == AnswerStatus.Approved);

                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new
                {
                    id            = user.Id,
                    firstName     = user.FirstName,
                    lastName      = user.LastName,
                    email         = user.Email,
                    userName      = user.UserName,
                    isActive      = user.IsActive,
                    createdAt     = user.CreatedAt,
                    bio           = profile?.Bio ?? string.Empty,
                    website       = profile?.Website ?? string.Empty,
                    location      = profile?.Location ?? string.Empty,
                    joinedAt      = profile?.JoinedAt ?? user.CreatedAt,
                    questionsCount,
                    answersCount,
                    roles
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in GetProfile");

                return StatusCode(500, new { message = "Error retrieving profile data",error = ex.Message ,  stack   = ex.StackTrace });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

                if (!int.TryParse(userIdStr, out var userId))
                    return Unauthorized(new { message = "Invalid user token" });

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return NotFound(new { message = "User not found" });

                // Update basic user
                user.FirstName = dto.FirstName;
                user.LastName  = dto.LastName;
                await _userManager.UpdateAsync(user);

                // Update or create profile entity
                var profile = await _context.UserProfiles
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (profile == null)
                {
                    profile = new UserProfile
                    {
                        UserId     = userId,
                        JoinedAt   = System.DateTime.UtcNow,
                        Bio        = dto.Bio,
                        Website    = dto.Website,
                        Location   = dto.Location,
                        
                    };
                    _context.UserProfiles.Add(profile);
                }
                else
                {
                    profile.Bio       = dto.Bio;
                    profile.Website   = dto.Website;
                    profile.Location  = dto.Location;
                    profile.JoinedAt = System.DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Profile updated" });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in UpdateProfile");
                return StatusCode(500, new { message = "Error updating profile", detail = ex.Message });
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
                                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

                if (!int.TryParse(userIdStr, out var userId))
                    return Unauthorized(new { message = "Invalid user token" });

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return NotFound(new { message = "User not found" });

                var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if (!result.Succeeded)
                    return BadRequest(new { message = "Password change failed", errors = result.Errors.Select(e => e.Description) });

                return Ok(new { message = "Password changed" });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in ChangePassword");
                return StatusCode(500, new { message = "Error changing password", detail = ex.Message });
            }
        }
    }
}
