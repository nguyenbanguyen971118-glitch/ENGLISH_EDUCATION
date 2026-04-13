using backend.DTOs;

namespace backend.Services.Interfaces;

public interface IFirebasePushService
{
    Task SendNewMessagePushAsync(ChatMessageDto message, IEnumerable<Guid> receiverUserIds, CancellationToken cancellationToken = default);
    Task RegisterDeviceTokenAsync(Guid userId, string token);
}
