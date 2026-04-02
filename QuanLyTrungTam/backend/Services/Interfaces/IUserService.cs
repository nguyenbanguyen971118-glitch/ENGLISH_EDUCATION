using backend.DTOs;

namespace backend.Services.Interfaces;

public interface IUserService
{
    Task<ApiResponseDto<AuthResultDto>> AuthenticateAsync(string email, string password);
    Task<ApiResponseDto<RefreshTokenResultDto>> RefreshAsync(string sessionId, string refreshToken);
    Task<ApiResponseDto<object>> RefreshPermissionsAsync(Guid userId);
    Task<ApiResponseDto<object>> LogoutAsync(string sessionId);
    Task<object> GetUserByIdAsync(Guid userId);
}
