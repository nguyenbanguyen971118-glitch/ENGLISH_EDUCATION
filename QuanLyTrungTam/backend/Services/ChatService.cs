using System;
using System.IO;
using backend.DTOs;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace backend.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IChatRealtimeNotifier _chatRealtimeNotifier;
    private readonly IFirebasePushService _firebasePushService;

    public ChatService(
        IChatRepository chatRepository,
        IChatRealtimeNotifier chatRealtimeNotifier,
        IFirebasePushService firebasePushService)
    {
        _chatRepository = chatRepository;
        _chatRealtimeNotifier = chatRealtimeNotifier;
        _firebasePushService = firebasePushService;
    }

    public async Task<ApiResponseDto<List<ChatConversationDto>>> GetConversationsAsync(Guid currentUserId)
    {
        var conversations = await _chatRepository.GetConversationsAsync(currentUserId);
        return ApiResponseDto<List<ChatConversationDto>>.Ok(conversations, "Lấy danh sách hội thoại thành công.");
    }

    public async Task<ApiResponseDto<List<ChatUserDto>>> GetChatUsersAsync(Guid currentUserId)
    {
        var users = await _chatRepository.GetChatUsersAsync(currentUserId);
        return ApiResponseDto<List<ChatUserDto>>.Ok(users, "Lấy danh sách người dùng nhắn tin thành công.");
    }

    public async Task<ApiResponseDto<ChatConversationDto>> GetConversationAsync(Guid currentUserId, Guid conversationId)
    {
        if (!await _chatRepository.IsUserInConversationAsync(conversationId, currentUserId))
        {
            return ApiResponseDto<ChatConversationDto>.Fail("Bạn không thuộc hội thoại này.", "CHAT_FORBIDDEN");
        }

        var conversation = await _chatRepository.GetConversationByIdAsync(conversationId, currentUserId);
        if (conversation == null)
        {
            return ApiResponseDto<ChatConversationDto>.Fail("Không tìm thấy hội thoại.", "CHAT_NOT_FOUND");
        }

        return ApiResponseDto<ChatConversationDto>.Ok(conversation, "Lấy hội thoại thành công.");
    }

    public async Task<ApiResponseDto<List<ChatMessageDto>>> GetMessagesAsync(Guid currentUserId, Guid conversationId, int take = 50)
    {
        if (!await _chatRepository.IsUserInConversationAsync(conversationId, currentUserId))
        {
            return ApiResponseDto<List<ChatMessageDto>>.Fail("Bạn không thuộc hội thoại này.", "CHAT_FORBIDDEN");
        }

        var messages = await _chatRepository.GetMessagesAsync(conversationId, Math.Clamp(take, 1, 200));
        return ApiResponseDto<List<ChatMessageDto>>.Ok(messages, "Lấy tin nhắn thành công.");
    }

    public async Task<ApiResponseDto<ChatConversationDto>> CreateDirectConversationAsync(Guid currentUserId, CreateDirectConversationRequestDto request)
    {
        if (request.RecipientUserId == Guid.Empty)
        {
            return ApiResponseDto<ChatConversationDto>.Fail("Thiếu người nhận tin nhắn.", "CHAT_RECIPIENT_REQUIRED");
        }

        if (request.RecipientUserId == currentUserId)
        {
            return ApiResponseDto<ChatConversationDto>.Fail("Không thể tạo hội thoại với chính mình.", "CHAT_INVALID_RECIPIENT");
        }

        if (!await _chatRepository.UserExistsAsync(request.RecipientUserId))
        {
            return ApiResponseDto<ChatConversationDto>.Fail("Người nhận không tồn tại.", "CHAT_RECIPIENT_NOT_FOUND");
        }

        var conversationId = await _chatRepository.FindDirectConversationIdAsync(currentUserId, request.RecipientUserId)
            ?? await _chatRepository.CreateConversationAsync(null, currentUserId, new[] { request.RecipientUserId });

        if (!string.IsNullOrWhiteSpace(request.InitialMessage))
        {
            var messageResult = await SendMessageAsync(currentUserId, conversationId, new SendMessageRequestDto
            {
                Content = request.InitialMessage
            });

            if (!messageResult.Success)
            {
                return ApiResponseDto<ChatConversationDto>.Fail(messageResult.Message, messageResult.Code);
            }
        }

        var conversation = await _chatRepository.GetConversationByIdAsync(conversationId, currentUserId);
        if (conversation == null)
        {
            return ApiResponseDto<ChatConversationDto>.Fail("Không tạo được hội thoại.", "CHAT_CREATE_FAILED");
        }

        await _chatRealtimeNotifier.NotifyConversationUpdatedAsync(conversationId, conversation.Members.Select(x => x.UserId));

        return ApiResponseDto<ChatConversationDto>.Ok(conversation, "Tạo hội thoại trực tiếp thành công.");
    }

    public async Task<ApiResponseDto<ChatConversationDto>> CreateGroupConversationAsync(Guid currentUserId, CreateGroupConversationRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return ApiResponseDto<ChatConversationDto>.Fail("Tên nhóm không được để trống.", "CHAT_GROUP_TITLE_REQUIRED");
        }

        var memberIds = request.MemberUserIds
            .Where(x => x != Guid.Empty && x != currentUserId)
            .Distinct()
            .ToList();

        if (!memberIds.Any())
        {
            return ApiResponseDto<ChatConversationDto>.Fail("Cần ít nhất một thành viên khác để tạo nhóm.", "CHAT_GROUP_MEMBERS_REQUIRED");
        }

        var profiles = await _chatRepository.GetUserProfilesAsync(memberIds);
        if (profiles.Count != memberIds.Count)
        {
            return ApiResponseDto<ChatConversationDto>.Fail("Danh sách thành viên có người dùng không hợp lệ.", "CHAT_GROUP_INVALID_MEMBER");
        }

        var conversationId = await _chatRepository.CreateConversationAsync(request.Title.Trim(), currentUserId, memberIds);

        if (!string.IsNullOrWhiteSpace(request.InitialMessage))
        {
            var messageResult = await SendMessageAsync(currentUserId, conversationId, new SendMessageRequestDto
            {
                Content = request.InitialMessage
            });

            if (!messageResult.Success)
            {
                return ApiResponseDto<ChatConversationDto>.Fail(messageResult.Message, messageResult.Code);
            }
        }

        var conversation = await _chatRepository.GetConversationByIdAsync(conversationId, currentUserId);
        if (conversation == null)
        {
            return ApiResponseDto<ChatConversationDto>.Fail("Không tạo được nhóm chat.", "CHAT_GROUP_CREATE_FAILED");
        }

        await _chatRealtimeNotifier.NotifyConversationUpdatedAsync(conversationId, conversation.Members.Select(x => x.UserId));

        return ApiResponseDto<ChatConversationDto>.Ok(conversation, "Tạo nhóm chat thành công.");
    }

    public async Task<ApiResponseDto<ChatMessageDto>> SendMessageAsync(Guid currentUserId, Guid conversationId, SendMessageRequestDto request)
    {
        var hasAttachmentUrls = request.AttachmentUrls != null && request.AttachmentUrls.Any();
        if (string.IsNullOrWhiteSpace(request.Content) && !hasAttachmentUrls)
        {
            return ApiResponseDto<ChatMessageDto>.Fail("Nội dung tin nhắn không được để trống.", "CHAT_MESSAGE_REQUIRED");
        }

        if (!await _chatRepository.IsUserInConversationAsync(conversationId, currentUserId))
        {
            return ApiResponseDto<ChatMessageDto>.Fail("Bạn không thuộc hội thoại này.", "CHAT_FORBIDDEN");
        }

        var message = await _chatRepository.CreateMessageAsync(conversationId, currentUserId, request.Content ?? string.Empty, hasAttachmentUrls ? request.AttachmentUrls : null);
        var conversation = await _chatRepository.GetConversationByIdAsync(conversationId, currentUserId);

        var memberIds = conversation?.Members.Select(x => x.UserId).Distinct().ToList() ?? new List<Guid>();
        var receiverIds = memberIds.Where(x => x != currentUserId).ToList();

        await _chatRealtimeNotifier.NotifyNewMessageAsync(message, memberIds);
        await _chatRealtimeNotifier.NotifyConversationUpdatedAsync(conversationId, memberIds);
        await _firebasePushService.SendNewMessagePushAsync(message, receiverIds);

        return ApiResponseDto<ChatMessageDto>.Ok(message, "Gửi tin nhắn thành công.");
    }

    public async Task<ApiResponseDto<object>> MarkAsReadAsync(Guid currentUserId, Guid conversationId)
    {
        if (!await _chatRepository.IsUserInConversationAsync(conversationId, currentUserId))
        {
            return ApiResponseDto<object>.Fail("Bạn không thuộc hội thoại này.", "CHAT_FORBIDDEN");
        }

        var updatedCount = await _chatRepository.MarkMessagesAsReadAsync(conversationId, currentUserId);

        var conversation = await _chatRepository.GetConversationByIdAsync(conversationId, currentUserId);
        var memberIds = conversation?.Members.Select(x => x.UserId).Distinct().ToList() ?? new List<Guid>();
        await _chatRealtimeNotifier.NotifyConversationUpdatedAsync(conversationId, memberIds);

        return ApiResponseDto<object>.Ok(new
        {
            conversationId,
            updatedCount
        }, "Đánh dấu đã đọc thành công.");
    }

    public async Task<ApiResponseDto<List<string>>> UploadMessageAttachmentsAsync(Guid currentUserId, List<IFormFile> files)
    {
        if (files == null || !files.Any())
        {
            return ApiResponseDto<List<string>>.Fail("Không có tệp đính kèm nào được gửi.", "CHAT_ATTACHMENTS_REQUIRED");
        }

        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "message-attachments");
        Directory.CreateDirectory(uploadFolder);

        var urls = new List<string>();
        foreach (var file in files)
        {
            if (file == null || file.Length == 0)
            {
                continue;
            }

            try
            {
                var extension = Path.GetExtension(file.FileName);
                var storedFileName = $"attachment_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadFolder, storedFileName);

                await using var fileStream = File.Create(filePath);
                await file.CopyToAsync(fileStream);

                urls.Add($"/uploads/message-attachments/{storedFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving attachment: {ex.Message}");
                // Continue with other files if one fails
            }
        }

        if (!urls.Any())
        {
            return ApiResponseDto<List<string>>.Fail("Không thể lưu tệp đính kèm.", "CHAT_ATTACHMENTS_SAVE_FAILED");
        }

        return ApiResponseDto<List<string>>.Ok(urls, "Tải tệp đính kèm lên thành công.");
    }

    public async Task<ApiResponseDto<object>> RegisterDeviceTokenAsync(Guid currentUserId, string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return ApiResponseDto<object>.Fail("Device token không hợp lệ.", "CHAT_DEVICE_TOKEN_INVALID");
        }

        await _firebasePushService.RegisterDeviceTokenAsync(currentUserId, token.Trim());

        return ApiResponseDto<object>.Ok(new
        {
            userId = currentUserId,
            registered = true
        }, "Đăng ký device token thành công.");
    }
}
