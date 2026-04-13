using backend.DTOs;
using backend.Hubs;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace backend.Services;

public class ChatRealtimeNotifier : IChatRealtimeNotifier
{
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatRealtimeNotifier(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyConversationUpdatedAsync(Guid conversationId, IEnumerable<Guid> memberUserIds)
    {
        var userIds = memberUserIds.Distinct().ToList();
        var tasks = new List<Task>
        {
            _hubContext.Clients.Group(ChatHub.GetConversationGroup(conversationId))
                .SendAsync("conversation-updated", new { conversationId })
        };

        tasks.AddRange(userIds.Select(userId =>
            _hubContext.Clients.Group(ChatHub.GetUserGroup(userId))
                .SendAsync("conversation-updated", new { conversationId })));

        await Task.WhenAll(tasks);
    }

    public async Task NotifyNewMessageAsync(ChatMessageDto message, IEnumerable<Guid> memberUserIds)
    {
        var userIds = memberUserIds.Distinct().ToList();

        var tasks = new List<Task>
        {
            _hubContext.Clients.Group(ChatHub.GetConversationGroup(message.ConversationId))
                .SendAsync("message-created", message)
        };

        tasks.AddRange(userIds.Select(userId =>
            _hubContext.Clients.Group(ChatHub.GetUserGroup(userId))
                .SendAsync("message-created", message)));

        await Task.WhenAll(tasks);
    }
}
