using backend.DTOs;

namespace backend.Services.Interfaces;

public interface IUserService
{
    Task<object> AuthenticateAsync(LoginDto loginData);
    Task<object> GetUserByIdAsync(Guid userId);
}
