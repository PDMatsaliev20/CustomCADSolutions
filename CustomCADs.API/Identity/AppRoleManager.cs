using CustomCADs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace CustomCADs.API.Identity
{
    public class AppRoleManager(
        IRoleStore<AppRole> store, 
        IEnumerable<IRoleValidator<AppRole>> roleValidators, 
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
        ILogger<AppRoleManager> logger) 
            : RoleManager<AppRole>(store, roleValidators, keyNormalizer, errors, logger)
    {
    }
}
