using backend.Data;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class DbRefreshSessionStore : IRefreshSessionStore
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DbRefreshSessionStore(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public string CreateOrReplace(Guid userId, string refreshToken, DateTime expiresAtUtc)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var sessionId = Guid.NewGuid().ToString("N");
        context.RefreshSessions.Add(new RefreshSession
        {
            SessionId = sessionId,
            UserId = userId,
            RefreshToken = refreshToken,
            ExpiresAtUtc = expiresAtUtc,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = null
        });
        context.SaveChanges();

        return sessionId;
    }

    public bool TryValidateAndRotate(string sessionId, string refreshToken, string newRefreshToken, DateTime newExpiresAtUtc, out Guid userId)
    {
        userId = Guid.Empty;

        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var session = context.RefreshSessions.FirstOrDefault(item => item.SessionId == sessionId);
        if (session == null)
        {
            return false;
        }

        if (!string.Equals(session.RefreshToken, refreshToken, StringComparison.Ordinal) || session.ExpiresAtUtc <= DateTime.UtcNow)
        {
            context.RefreshSessions.Remove(session);
            context.SaveChanges();
            return false;
        }

        userId = session.UserId;
        session.RefreshToken = newRefreshToken;
        session.ExpiresAtUtc = newExpiresAtUtc;
        session.UpdatedAtUtc = DateTime.UtcNow;
        context.SaveChanges();
        return true;
    }

    public void Revoke(string sessionId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var session = context.RefreshSessions.FirstOrDefault(item => item.SessionId == sessionId);
        if (session != null)
        {
            context.RefreshSessions.Remove(session);
            context.SaveChanges();
        }
    }
}