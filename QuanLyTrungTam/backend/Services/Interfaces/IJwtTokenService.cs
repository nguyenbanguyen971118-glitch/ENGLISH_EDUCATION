namespace backend.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId, string email, string role, string fullName);
    string GenerateRefreshToken();
}
