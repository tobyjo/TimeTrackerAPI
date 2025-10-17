using System.Security.Claims;

namespace TimeTracker.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        // Prefer Auth0 "sub" claim; fallback to ClaimTypes.NameIdentifier
        public static string? GetAuth0UserId(this ClaimsPrincipal user)
        {
            if (user == null) return null;
            var sub = user.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(sub)) return sub;
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
