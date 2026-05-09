using backend.DTOs;

namespace backend.Services.Interfaces
{
    public interface INotificationService
    {
        // Lấy tất cả thông báo (chỉ admin)
        Task<ApiResponseDto<List<NotificationDto>>> GetAllAsync();
        
        // Lấy thông báo của người dùng hiện tại
        Task<ApiResponseDto<List<UserNotificationDto>>> GetUserNotificationsAsync(Guid userId);
        
        // Lấy thông báo chưa đọc của người dùng
        Task<ApiResponseDto<List<UserNotificationDto>>> GetUnreadNotificationsAsync(Guid userId);
        
        // Đếm thông báo chưa đọc
        Task<ApiResponseDto<int>> GetUnreadCountAsync(Guid userId);
        
        // Đánh dấu một thông báo là đã đọc
        Task<ApiResponseDto<object>> MarkAsReadAsync(Guid userId, Guid notificationId);
        
        // Đánh dấu tất cả thông báo là đã đọc
        Task<ApiResponseDto<object>> MarkAllAsReadAsync(Guid userId);
        
        // Tạo thông báo mới
        Task<ApiResponseDto<NotificationDto>> CreateAsync(Guid currentUserId, CreateNotificationDto dto);
        
        // Cập nhật thông báo
        Task<ApiResponseDto<NotificationDto>> UpdateAsync(Guid currentUserId, UpdateNotificationDto dto);
        
        // Xóa thông báo
        Task<ApiResponseDto<object>> DeleteAsync(Guid id, Guid currentUserId);
    }
}
