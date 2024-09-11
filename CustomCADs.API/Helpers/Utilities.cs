using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using Microsoft.AspNetCore.JsonPatch;
using System.Security.Claims;
using static CustomCADs.Domain.DataConstants.UserConstants;

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

        public static async Task<(string value, DateTime end)> RenewRefreshToken(this IUserService userService, UserModel user)
        {
            string newRT = JwtHelper.GenerateRefreshToken();
            DateTime newEndDate = DateTime.UtcNow.AddDays(RefreshTokenDaysLimit);

            user.RefreshToken = newRT;
            user.RefreshTokenEndDate = newEndDate;
            await userService.EditAsync(user.Id, user).ConfigureAwait(false);

            return (newRT, newEndDate);
        }

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
