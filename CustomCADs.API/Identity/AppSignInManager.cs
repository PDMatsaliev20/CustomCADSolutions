using CustomCADs.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CustomCADs.API.Identity
{
    public class AppSignInManager(
        AppUserManager appUserManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<AppUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<AppSignInManager> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<AppUser> confirmation) 
            : SignInManager<AppUser>(appUserManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }
}
