using System.Collections.Concurrent;
using backend.Services.Interfaces;

namespace backend.Services;

public class InMemoryRefreshSessionStore : IRefreshSessionStore
{
    // Luu thong tin phien refresh token cua mot nguoi dung.
    private sealed class SessionRecord
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }

    // Session duoc luu trong bo nho, key la sessionId.
    private static readonly ConcurrentDictionary<string, SessionRecord> Sessions = new();

    // Tao session moi hoac thay session cu (theo sessionId moi) cho user.
    public string CreateOrReplace(Guid userId, string refreshToken, DateTime expiresAtUtc)
    {
        var sessionId = Guid.NewGuid().ToString("N");
        Sessions[sessionId] = new SessionRecord
        {
            UserId = userId,
            RefreshToken = refreshToken,
            ExpiresAtUtc = expiresAtUtc
        };
        return sessionId;
    }

    // Kiem tra refresh token hien tai, neu hop le thi xoay vong token moi.
    public bool TryValidateAndRotate(string sessionId, string refreshToken, string newRefreshToken, DateTime newExpiresAtUtc, out Guid userId)
    {
        userId = Guid.Empty;

        // Khong tim thay session.
        if (!Sessions.TryGetValue(sessionId, out var record))
        {
            return false;
        }

        // Token sai hoac het han: xoa session de tranh tai su dung.
        if (record.RefreshToken != refreshToken || record.ExpiresAtUtc <= DateTime.UtcNow)
        {
            Sessions.TryRemove(sessionId, out _);
            return false;
        }

        // Hop le: cap nhat refresh token va han moi cho session.
        userId = record.UserId;
        record.RefreshToken = newRefreshToken;
        record.ExpiresAtUtc = newExpiresAtUtc;
        Sessions[sessionId] = record;
        return true;
    }

    // Vo hieu hoa phien refresh token bang cach xoa session.
    public void Revoke(string sessionId)
    {
        Sessions.TryRemove(sessionId, out _);
    }
}
