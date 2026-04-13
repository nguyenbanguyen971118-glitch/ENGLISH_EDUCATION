using backend.DTOs;

namespace backend.Services.Interfaces;

public interface IChatService
{
    Task<ApiResponseDto<List<ChatUserDto>>> GetChatUsersAsync(Guid currentUserId);
    Task<ApiResponseDto<List<ChatConversationDto>>> GetConversationsAsync(Guid currentUserId);
    Task<ApiResponseDto<ChatConversationDto>> GetConversationAsync(Guid currentUserId, Guid conversationId);
    Task<ApiResponseDto<List<ChatMessageDto>>> GetMessagesAsync(Guid currentUserId, Guid conversationId, int take = 50);

    Task<ApiResponseDto<ChatConversationDto>> CreateDirectConversationAsync(Guid currentUserId, CreateDirectConversationRequestDto request);
    Task<ApiResponseDto<ChatConversationDto>> CreateGroupConversationAsync(Guid currentUserId, CreateGroupConversationRequestDto request);

    Task<ApiResponseDto<ChatMessageDto>> SendMessageAsync(Guid currentUserId, Guid conversationId, SendMessageRequestDto request);
    Task<ApiResponseDto<object>> MarkAsReadAsync(Guid currentUserId, Guid conversationId);

    Task<ApiResponseDto<object>> RegisterDeviceTokenAsync(Guid currentUserId, string token);
}
