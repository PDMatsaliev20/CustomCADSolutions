using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Stripe;

namespace CustomCADSolutions.App.Extensions
{
    public static class UtilityExtensions
    {
        public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public static IEnumerable<string> GetErrors(this ModelStateDictionary model) => model.Values.Select(v => v.Errors).SelectMany(ec => ec.Select(e => e.ErrorMessage));

        public static string GetFilePath(this IWebHostEnvironment env, string fullName)
            => Path.Combine(env.WebRootPath, "others", "cads", fullName);

        public static string GetFileExtension(this IFormFile cad) 
            => Path.GetExtension(cad.FileName);

        public static async Task<string?> UploadFileAsync(this IWebHostEnvironment env, IFormFile cad, string filePath)
        {
            if (cad != null && cad.Length != 0)
            {
                string fullPath = env.GetFilePath(filePath);
                using FileStream stream = new(fullPath, FileMode.Create);
                await cad.CopyToAsync(stream);

                return fullPath;
            }
            else return null;
        }

        public static void DeleteFile(this IWebHostEnvironment env, string name, string extension)
        {
            string filePath = env.GetFilePath(name + extension);
            System.IO.File.Delete(filePath);
        }

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

        public static bool ProcessPayment(this StripeSettings stripeSettings, string stripeToken, string name, decimal price)
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
