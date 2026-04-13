using backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.GetUserId();
        if (userId.HasValue)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetUserGroup(userId.Value));
        }

        await base.OnConnectedAsync();
    }

    public Task JoinConversation(Guid conversationId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, GetConversationGroup(conversationId));
    }

    public Task LeaveConversation(Guid conversationId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, GetConversationGroup(conversationId));
    }

    public static string GetUserGroup(Guid userId) => $"user:{userId:D}";
    public static string GetConversationGroup(Guid conversationId) => $"conversation:{conversationId:D}";
}
