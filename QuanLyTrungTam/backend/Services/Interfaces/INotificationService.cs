using backend.DTOs;

namespace backend.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResponseDto<List<NotificationDto>>> GetAllAsync();
        Task<ApiResponseDto<NotificationDto>> CreateAsync(Guid currentUserId, CreateNotificationDto dto);
        Task<ApiResponseDto<NotificationDto>> UpdateAsync(Guid currentUserId, UpdateNotificationDto dto);
        Task<ApiResponseDto<object>> DeleteAsync(Guid id, Guid currentUserId);
    }
}
