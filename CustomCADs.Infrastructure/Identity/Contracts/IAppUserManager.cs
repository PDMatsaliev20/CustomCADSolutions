using Microsoft.AspNetCore.Identity;

namespace CustomCADs.Infrastructure.Identity.Contracts
{
    public interface IAppUserManager
    {
        Task<AppUser?> FindByNameAsync(string username);
        Task<IdentityResult> CreateAsync(AppUser user);
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task UpdateAsync(AppUser user);
        Task AddToRoleAsync(AppUser user, string role);
        Task RemoveFromRoleAsync(AppUser user, string oldRole);
        Task DeleteAsync(AppUser user);
        Task<bool> IsLockedOutAsync(AppUser user);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<IdentityResult> ConfirmEmailAsync(AppUser user, string token);
    }
}
