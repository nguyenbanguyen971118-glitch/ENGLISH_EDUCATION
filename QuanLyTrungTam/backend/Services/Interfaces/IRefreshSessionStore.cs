namespace backend.Services.Interfaces;

public interface IRefreshSessionStore
{
    string CreateOrReplace(Guid userId, string refreshToken, DateTime expiresAtUtc);
    bool TryValidateAndRotate(string sessionId, string refreshToken, string newRefreshToken, DateTime newExpiresAtUtc, out Guid userId);
    void Revoke(string sessionId);
}
