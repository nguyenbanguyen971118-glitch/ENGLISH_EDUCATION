using backend.Helpers;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;

/// <summary>
/// Hub để gửi và nhận thông báo real-time
/// Người dùng sẽ được thêm vào các group theo vai trò của họ để nhận thông báo phù hợp
/// </summary>
[Authorize]
public class NotificationHub : Hub
{
    private readonly INotificationService _notificationService;

    public NotificationHub(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.GetUserId();
        
        if (userId.HasValue)
        {
            // Thêm người dùng vào group cá nhân
            await Groups.AddToGroupAsync(Context.ConnectionId, GetUserGroup(userId.Value));
            
            // Thêm người dùng vào group theo vai trò
            var userRoles = Context.User?.FindAll("role");
            if (userRoles != null)
            {
                foreach (var role in userRoles)
                {
                    var roleName = role.Value;
                    await Groups.AddToGroupAsync(Context.ConnectionId, GetRoleGroup(roleName));
                }
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.GetUserId();
        
        if (userId.HasValue)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetUserGroup(userId.Value));
            
            var userRoles = Context.User?.FindAll("role");
            if (userRoles != null)
            {
                foreach (var role in userRoles)
                {
                    var roleName = role.Value;
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetRoleGroup(roleName));
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Client gọi để đánh dấu một thông báo là đã đọc
    /// </summary>
    public async Task MarkNotificationAsRead(Guid notificationId)
    {
        var userId = Context.User?.GetUserId();
        if (userId.HasValue)
        {
            await _notificationService.MarkAsReadAsync(userId.Value, notificationId);
            
            // Thông báo cho client
            await Clients.Caller.SendAsync("NotificationMarkedAsRead", notificationId);
        }
    }

    /// <summary>
    /// Client gọi để lấy số lượng thông báo chưa đọc
    /// </summary>
    public async Task GetUnreadCount()
    {
        var userId = Context.User?.GetUserId();
        if (userId.HasValue)
        {
            var result = await _notificationService.GetUnreadCountAsync(userId.Value);
            if (result.Success)
            {
                await Clients.Caller.SendAsync("UnreadCountUpdated", result.Data);
            }
        }
    }

    // Static helper methods để generate tên group
    public static string GetUserGroup(Guid userId) => $"user:{userId:D}";
    public static string GetRoleGroup(string roleName) => $"role:{roleName}";
    
    // Cho admin sử dụng
    public static string GetAdminGroup() => "role:Admin";
    public static string GetTeacherGroup() => "role:Giao_Vien";
    public static string GetStudentGroup() => "role:Hoc_Sinh";
    public static string GetParentGroup() => "role:Phu_Huynh";
    public static string GetAllUsersGroup() => "all-users";
}
