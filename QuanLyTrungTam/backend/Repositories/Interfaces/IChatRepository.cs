using backend.DTOs;

namespace backend.Repositories.Interfaces;

public interface IChatRepository
{
    Task<List<ChatUserDto>> GetChatUsersAsync(Guid currentUserId);
    Task<bool> UserExistsAsync(Guid userId);
    Task<ChatUserDto?> GetUserProfileAsync(Guid userId);
    Task<List<ChatUserDto>> GetUserProfilesAsync(IEnumerable<Guid> userIds);

    Task<Guid?> FindDirectConversationIdAsync(Guid userA, Guid userB);
    Task<Guid> CreateConversationAsync(string? title, Guid creatorId, IEnumerable<Guid> memberIds);

    Task<bool> IsUserInConversationAsync(Guid conversationId, Guid userId);
    Task<ChatConversationDto?> GetConversationByIdAsync(Guid conversationId, Guid currentUserId);
    Task<List<ChatConversationDto>> GetConversationsAsync(Guid currentUserId);

    Task<ChatMessageDto> CreateMessageAsync(Guid conversationId, Guid senderId, string content);
    Task<List<ChatMessageDto>> GetMessagesAsync(Guid conversationId, int take);
    Task<int> MarkMessagesAsReadAsync(Guid conversationId, Guid currentUserId);
}
