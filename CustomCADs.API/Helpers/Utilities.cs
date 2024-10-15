using System.Security.Claims;

namespace CustomCADs.API.Helpers;

public static class Utilities
{
    public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    public static string GetName(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
    public static bool GetIsAuthenticated(this ClaimsPrincipal user) => user.Identity?.IsAuthenticated ?? false;
}
