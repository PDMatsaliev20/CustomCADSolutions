using CustomCADs.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using System.Security.Claims;

namespace CustomCADs.API.Helpers
{
    public static class Utilities
    {
        public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public static string GetMessage(this Exception ex) => $"{ex.GetType()}: {ex.Message}";

        public static string Capitalize(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length == 1)
            {
                return text.ToUpper();
            }

            return text[..1].ToUpper() + text[1..].ToLower();
        }

        public static async Task SignInAsync(this AppUser user, SignInManager<AppUser> signInManager, AuthenticationProperties? authprop)
            => await signInManager.Context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                await signInManager.CreateUserPrincipalAsync(user).ConfigureAwait(false),
                authprop).ConfigureAwait(false);

        public static AuthenticationProperties GetAuthProps(bool rememberMe) => rememberMe ?
            new()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
            } : new()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
            };

        public static string? CheckForBadChanges<TModel>(this JsonPatchDocument<TModel> patchDoc, params string[] fields) where TModel : class
        {
            foreach (string field in fields)
            {
                return patchDoc.Operations.FirstOrDefault(op => op.path == field)?.path;
            }
            
            return null;
        }
    }
}
