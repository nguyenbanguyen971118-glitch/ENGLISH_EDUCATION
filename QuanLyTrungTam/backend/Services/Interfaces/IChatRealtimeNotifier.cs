using backend.DTOs;

namespace backend.Services.Interfaces;

public interface IChatRealtimeNotifier
{
    Task NotifyConversationUpdatedAsync(Guid conversationId, IEnumerable<Guid> memberUserIds);
    Task NotifyNewMessageAsync(ChatMessageDto message, IEnumerable<Guid> memberUserIds);
}
