using backend.Models;

namespace backend.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Thongbao>> GetAllActiveAsync();
        Task<Thongbao?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(Thongbao thongBao, List<Guid> receiverIds);
        Task UpdateAsync(Thongbao thongBao, List<Guid> newReceiverIds, Guid currentUserId);
        Task DeleteSoftAsync(Guid id, Guid currentUserId);
    }
}
