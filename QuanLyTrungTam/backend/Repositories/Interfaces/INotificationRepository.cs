using backend.Models;

namespace backend.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Thongbao>> GetAllActiveAsync();
        Task<Thongbao?> GetByIdAsync(Guid id);
        
        Task<List<Nguoinhanthongbao>> GetNotificationsByUserIdAsync(Guid userId);
        Task<List<Nguoinhanthongbao>> GetUnreadNotificationsByUserIdAsync(Guid userId);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task MarkAsReadAsync(Guid userId, Guid notificationId);
        Task MarkAllAsReadAsync(Guid userId);
        
        Task<Guid> CreateAsync(Thongbao thongBao, List<Guid> receiverIds);
        Task UpdateAsync(Thongbao thongBao, List<Guid> newReceiverIds, Guid currentUserId);
        Task DeleteSoftAsync(Guid id, Guid currentUserId);
    }
}
