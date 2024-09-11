using CustomCADs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CustomCADs.API.Identity
{
    public class AppUserManager(
        IUserStore<AppUser> store, 
        IOptions<IdentityOptions> optionsAccessor, 
        IPasswordHasher<AppUser> passwordHasher, 
        IEnumerable<IUserValidator<AppUser>> userValidators, 
        IEnumerable<IPasswordValidator<AppUser>> passwordValidators, 
        ILookupNormalizer keyNormalizer, 
        IdentityErrorDescriber errors, 
        IServiceProvider services, 
        ILogger<AppUserManager> logger) 
            : UserManager<AppUser>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }
}
