using Microsoft.AspNetCore.Identity;

namespace CustomCADs.Auth.Contracts;

public interface IAppUserManager
{
    Task<AppUser?> FindByIdAsync(string id);
    Task<AppUser?> FindByNameAsync(string username);
    Task<AppUser?> FindByEmailAsync(string email);
    Task<IdentityResult> CreateAsync(AppUser user);
    Task<IdentityResult> CreateAsync(AppUser user, string password);
    Task UpdateAsync(AppUser user);
    Task AddToRoleAsync(AppUser user, string role);
    Task RemoveFromRoleAsync(AppUser user, string oldRole);
    Task DeleteAsync(AppUser user);
    Task<bool> IsLockedOutAsync(AppUser user);
    Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
    Task<IdentityResult> ConfirmEmailAsync(AppUser user, string token);
    Task<string> GeneratePasswordResetTokenAsync(AppUser user);
    Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string newPassword);
}
