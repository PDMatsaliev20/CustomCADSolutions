using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using Stripe;

namespace CustomCADSolutions.App.Extensions
{
    public static class UtilityExtensions
    {
        public static bool ProcessPayment(this StripeSettings stripeSettings, string stripeToken)
        {
            StripeConfiguration.ApiKey = stripeSettings.TestSecretKey;
            ChargeCreateOptions options = new()
            {
                Amount = 100,
                Currency = "bgn",
                Source = stripeToken,
                Description = "Example Charge",
            };
            Charge charge = new ChargeService().Create(options);

            return charge.Status == "succeeded";
        }

        public static IEnumerable<string> GetErrors(this ModelStateDictionary model) => model.Values.Select(v => v.Errors).SelectMany(ec => ec.Select(e => e.ErrorMessage));

        public static string SecureQuery(this HttpContext httpContext, params KeyValuePair<string, string>[] kvpairs)
        {
            string? query = httpContext.Request.QueryString.Value;
            query = string.IsNullOrWhiteSpace(query) ? "?" : query;
            foreach (KeyValuePair<string, string> kvp in kvpairs)
            {
                query = query.Insert(1, $"{kvp.Key}={kvp.Value}&");
            }
            return query;
        }

        public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier);

        public static async Task<byte[]> GetBytesAsync(this IFormFile cad)
{
    using MemoryStream memoryStream = new();
    await cad.CopyToAsync(memoryStream);
    byte[] fileBytes = memoryStream.ToArray();

    return fileBytes;
}
        
        public static async Task<T?> TryGetFromJsonAsync<T>(this HttpClient httpClient, string path) where T : class
    => await httpClient.GetFromJsonAsync(path, typeof(T)) as T;
    }
}
