using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backend.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var rawValue = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(rawValue, out var userId) ? userId : null;
    }

    public static Guid? GetProfileId(this ClaimsPrincipal principal)
    {
        var rawValue = principal.FindFirstValue("profileId");
        return Guid.TryParse(rawValue, out var profileId) ? profileId : null;
    }
}
