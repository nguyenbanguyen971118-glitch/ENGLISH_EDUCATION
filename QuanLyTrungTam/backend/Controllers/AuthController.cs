using Microsoft.AspNetCore.Mvc;
using backend.Services.Interfaces;
using backend.DTOs;
using System.Text.Json;

namespace backend.Controllers
{
    /// <summary>
    /// Controller xử lý các nghiệp vụ xác thực: đăng nhập, làm mới token, đăng xuất
    /// và cập nhật lại quyền trong phiên làm việc.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JsonElement payload)
        {
            // Tất cả request của FE được chuẩn hoá theo format { data: { ... } }.
            // Dùng JsonElement để tránh tạo nhiều DTO riêng cho từng endpoint auth.
            if (!TryReadField(payload, "email", out var email) || !TryReadField(payload, "password", out var password))
            {
                return BadRequest(ApiResponseDto<object>.Fail("Thiếu email hoặc mật khẩu.", "AUTH_INVALID_INPUT"));
            }

            var result = await _userService.AuthenticateAsync(email, password);
            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] JsonElement payload)
        {
            // Endpoint refresh dùng cùng cấu trúc payload.data để FE không phải tách chuẩn gửi dữ liệu.
            if (!TryReadField(payload, "sessionId", out var sessionId) || !TryReadField(payload, "refreshToken", out var refreshToken))
            {
                return BadRequest(ApiResponseDto<object>.Fail("Thiếu sessionId hoặc refreshToken.", "AUTH_REFRESH_INVALID_INPUT"));
            }

            var result = await _userService.RefreshAsync(sessionId, refreshToken);
            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Đăng xuất bằng cách vô hiệu hóa session refresh token ở server.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] JsonElement payload)
        {
            if (!TryReadField(payload, "sessionId", out var sessionId))
            {
                return BadRequest(ApiResponseDto<object>.Fail("Thiếu sessionId.", "AUTH_LOGOUT_INVALID_INPUT"));
            }

            var result = await _userService.LogoutAsync(sessionId);
            return Ok(result);
        }

        /// <summary>
        /// Cập nhật lại danh sách quyền của một người dùng sau khi admin thay đổi phân quyền.
        /// Endpoint này giúp FE refresh dữ liệu mà không cần bắt người dùng đăng nhập lại.
        /// </summary>
        [HttpPost("refresh-permissions/{userId:guid}")]
        public async Task<IActionResult> RefreshPermissions(Guid userId)
        {
            var result = await _userService.RefreshPermissionsAsync(userId);
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        private static bool TryReadField(JsonElement payload, string fieldName, out string value)
        {
            value = string.Empty;

            // Contract chung của auth API: mọi input phải nằm trong object "data".
            if (!payload.TryGetProperty("data", out var dataObj) || dataObj.ValueKind != JsonValueKind.Object)
            {
                return false;
            }

            if (!dataObj.TryGetProperty(fieldName, out var field) || field.ValueKind != JsonValueKind.String)
            {
                return false;
            }

            value = field.GetString()?.Trim() ?? string.Empty;
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}