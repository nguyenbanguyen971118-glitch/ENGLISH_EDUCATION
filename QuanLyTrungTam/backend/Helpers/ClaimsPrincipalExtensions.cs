using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backend.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var rawValue = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? principal.FindFirstValue("sub");

        return Guid.TryParse(rawValue, out var userId) ? userId : null;
    }
}
