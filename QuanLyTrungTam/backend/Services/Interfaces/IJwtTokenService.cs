namespace backend.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId, string email, string role, string fullName);
    string GenerateAccessToken(Guid userId, string email, string role, string fullName, Guid? profileId);
    string GenerateRefreshToken();
}
