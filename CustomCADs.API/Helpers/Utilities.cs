using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Stripe;
using System.Security.Claims;

namespace CustomCADs.API.Helpers
{
    public static class Utilities
    {
        public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier)!;

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
                await signInManager.CreateUserPrincipalAsync(user),
                authprop);

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

        public static bool ProcessPayment(this StripeInfo stripeSettings, string stripeToken, string buyer, CadModel cad)
        {
            StripeConfiguration.ApiKey = stripeSettings.TestSecretKey;
            Charge charge = new ChargeService().Create(new()
            {
                Amount = (long)(cad.Price * 100),
                Currency = "USD",
                Source = stripeToken,
                Description = $"{buyer} bought {cad.Creator.UserName}'s {cad.Name} for {cad.Price}$.",
            });

            return charge.Status == "succeeded";
        }
    }
}
