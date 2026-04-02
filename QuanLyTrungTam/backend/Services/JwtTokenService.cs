using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using backend.Services.Interfaces;

namespace backend.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    // - chuc nang: Khoi tao service va nap cau hinh JWT.
    // - nmkhue -29/2/2026
    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // - chuc nang: Tao access token chua thong tin nguoi dung va vai tro.
    // - nmkhue -29/2/2026
    public string GenerateAccessToken(Guid userId, string email, string role, string fullName)
    {
        var jwtSection = _configuration.GetSection("JwtSettings");
        var secret = jwtSection["Secret"] ?? throw new InvalidOperationException("Missing JwtSettings:Secret");
        var issuer = jwtSection["Issuer"] ?? "backend";
        var audience = jwtSection["Audience"] ?? "frontend";
        var expiresMinutes = int.TryParse(jwtSection["AccessTokenMinutes"], out var parsedMinutes) ? parsedMinutes : 15;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.Role, role),
            new("fullName", fullName)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // - chuc nang: Tao refresh token ngau nhien de cap phien dang nhap.
    // - nmkhue -29/2/2026
    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}
