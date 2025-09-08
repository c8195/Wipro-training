using DoConnect.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var notifications = await _notificationService.GetUserNotificationsAsync(userId, page, pageSize);
                
                return Ok(new { notifications, page, pageSize });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Error retrieving notifications" });
            }
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _notificationService.MarkAsReadAsync(id, userId);
                
                return Ok(new { message = "Notification marked as read" });
            }
            catch (Exception )
            {
                return StatusCode(500, new { message = "Error marking notification as read" });
            }
        }

        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _notificationService.MarkAllAsReadAsync(userId);
                
                return Ok(new { message = "All notifications marked as read" });
            }
            catch (Exception )
            {
                return StatusCode(500, new { message = "Error marking notifications as read" });
            }
        }
    }
}
