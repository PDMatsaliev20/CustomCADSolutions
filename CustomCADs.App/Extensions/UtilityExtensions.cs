using CustomCADs.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stripe;
using System.Security.Claims;

namespace CustomCADs.App.Extensions
{
    public static class UtilityExtensions
    {
        public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public static IEnumerable<string> GetErrors(this ModelStateDictionary model) => model.Values.Select(v => v.Errors).SelectMany(ec => ec.Select(e => e.ErrorMessage));

        public static async Task AddUserAsync(this UserManager<AppUser> userManager, string username, string email, string password, string role)
        {
            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    AppUser user = new()
                    {
                        UserName = username,
                        Email = email,
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }

        public static bool ProcessPayment(this StripeInfo stripeSettings, string stripeToken, string name, decimal price)
        {
            StripeConfiguration.ApiKey = stripeSettings.TestSecretKey;
            ChargeCreateOptions options = new()
            {
                Amount = (long)(price * 100),
                Currency = "bgn",
                Source = stripeToken,
                Description = $"Bought {name} for {price}.",
            };
            Charge charge = new ChargeService().Create(options);

            return charge.Status == "succeeded";
        }
    }
}
