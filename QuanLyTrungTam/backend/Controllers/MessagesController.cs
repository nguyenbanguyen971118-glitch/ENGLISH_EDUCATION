using backend.Attributes;
using backend.DTOs;
using backend.Helpers;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Authorize(Roles = "Admin,Giao_Vien,Phu_Huynh,Hoc_Sinh")]
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IChatService _chatService;

    public MessagesController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetChatUsers()
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Phiên đăng nhập không hợp lệ.", "AUTH_INVALID_TOKEN"));
        }

        var result = await _chatService.GetChatUsersAsync(userId.Value);
        return Ok(result);
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations()
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Phiên đăng nhập không hợp lệ.", "AUTH_INVALID_TOKEN"));
        }

        var result = await _chatService.GetConversationsAsync(userId.Value);
        return Ok(result);
    }

    [HttpGet("conversations/{conversationId:guid}")]
    public async Task<IActionResult> GetConversation(Guid conversationId)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Phiên đăng nhập không hợp lệ.", "AUTH_INVALID_TOKEN"));
        }

        var result = await _chatService.GetConversationAsync(userId.Value, conversationId);
        if (!result.Success && result.Code == "CHAT_FORBIDDEN")
        {
            return Forbid();
        }

        if (!result.Success && result.Code == "CHAT_NOT_FOUND")
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpGet("conversations/{conversationId:guid}/messages")]
    public async Task<IActionResult> GetMessages(Guid conversationId, [FromQuery] int take = 50)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Phiên đăng nhập không hợp lệ.", "AUTH_INVALID_TOKEN"));
        }

        var result = await _chatService.GetMessagesAsync(userId.Value, conversationId, take);
        if (!result.Success && result.Code == "CHAT_FORBIDDEN")
        {
            return Forbid();
        }

        return Ok(result);
    }

    [HttpPost("direct")]
    public async Task<IActionResult> CreateDirectConversation([FromBody] CreateDirectConversationRequestDto request)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Phiên đăng nhập không hợp lệ.", "AUTH_INVALID_TOKEN"));
        }

        var result = await _chatService.CreateDirectConversationAsync(userId.Value, request);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("groups")]
    public async Task<IActionResult> CreateGroupConversation([FromBody] CreateGroupConversationRequestDto request)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Phiên đăng nhập không hợp lệ.", "AUTH_INVALID_TOKEN"));
        }

        var result = await _chatService.CreateGroupConversationAsync(userId.Value, request);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("conversations/{conversationId:guid}/messages")]
    public async Task<IActionResult> SendMessage(Guid conversationId, [FromBody] SendMessageRequestDto request)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Phiên đăng nhập không hợp lệ.", "AUTH_INVALID_TOKEN"));
        }

        var result = await _chatService.SendMessageAsync(userId.Value, conversationId, request);
        if (!result.Success && result.Code == "CHAT_FORBIDDEN")
        {
            return Forbid();
        }

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("attachments")]
    public async Task<IActionResult> UploadAttachments([FromForm] List<IFormFile> files)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Phiên đăng nhập không hợp lệ.", "AUTH_INVALID_TOKEN"));
        }

        var result = await _chatService.UploadMessageAttachmentsAsync(userId.Value, files);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("conversations/{conversationId:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid conversationId)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Phiên đăng nhập không hợp lệ.", "AUTH_INVALID_TOKEN"));
        }

        var result = await _chatService.MarkAsReadAsync(userId.Value, conversationId);
        if (!result.Success && result.Code == "CHAT_FORBIDDEN")
        {
            return Forbid();
        }

        return Ok(result);
    }

    [HttpPost("device-token")]
    public async Task<IActionResult> RegisterDeviceToken([FromBody] RegisterDeviceTokenRequestDto request)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return Unauthorized(ApiResponseDto<object>.Fail("Phiên đăng nhập không hợp lệ.", "AUTH_INVALID_TOKEN"));
        }

        var result = await _chatService.RegisterDeviceTokenAsync(userId.Value, request.DeviceToken);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
