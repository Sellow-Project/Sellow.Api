using System.Security.Claims;

namespace Sellow.Shared.Infrastructure.Auth;

public static class Extensions
{
    public static Guid AuthenticatedUserId(this ClaimsPrincipal principal)
        => Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
}