using backend.DTOs;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Yêu cầu phải đăng nhập
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }   

        /// <summary>
        /// Lấy tất cả thông báo (chỉ Admin)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _notificationService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông báo của người dùng hiện tại
        /// </summary>
        [HttpGet("my-notifications")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông báo chưa đọc của người dùng hiện tại
        /// </summary>
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await _notificationService.GetUnreadNotificationsAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Đếm số lượng thông báo chưa đọc của người dùng hiện tại
        /// </summary>
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Đánh dấu một thông báo là đã đọc
        /// </summary>
        [HttpPost("{id}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await _notificationService.MarkAsReadAsync(userId, id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Đánh dấu tất cả thông báo là đã đọc
        /// </summary>
        [HttpPost("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var result = await _notificationService.MarkAllAsReadAsync(userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Tạo thông báo mới (chỉ Admin). Hỗ trợ đính kèm nhiều file (Word, Excel, PowerPoint, PDF).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [RequestSizeLimit(250_000_000)]
        public async Task<IActionResult> Create([FromForm] CreateNotificationDto dto)
        {
            var result = await _notificationService.CreateAsync(GetCurrentUserId(), dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Tải file đính kèm của một thông báo. Admin tải được mọi file;
        /// người dùng khác chỉ tải được file của thông báo mình là người nhận.
        /// </summary>
        [HttpGet("attachments/{resourceId:guid}/download")]
        public async Task<IActionResult> DownloadAttachment(Guid resourceId)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var (physicalPath, fileName, error) = await _notificationService.GetAttachmentForDownloadAsync(
                userId, User.IsInRole("Admin"), resourceId);

            if (error != null)
            {
                return error.ErrorCode == "ATTACHMENT_FORBIDDEN" ? StatusCode(403, error) : NotFound(error);
            }

            var contentTypeProvider = new FileExtensionContentTypeProvider();
            if (!contentTypeProvider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(physicalPath!, contentType, fileName);
        }

        /// <summary>
        /// Cập nhật thông báo (chỉ Admin)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNotificationDto dto)
        {
            dto.Id = id; // Đảm bảo ID đồng nhất
            var result = await _notificationService.UpdateAsync(GetCurrentUserId(), dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Xóa thông báo (chỉ Admin)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _notificationService.DeleteAsync(id, GetCurrentUserId());
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
