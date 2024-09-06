using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Users;
using CustomCADs.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.API.Helpers.Utilities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;
    using static ApiMessages;

    /// <summary>
    ///     Controller for managing Authentication/Authorization and info about the User's Identity.
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    [ApiController]
    [Route("API/[controller]")]
    public class IdentityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : ControllerBase
    {
        /// <summary>
        ///     Creates a new account with the specified parameters for the user and logs into it.
        /// </summary>
        /// <param name="role">Must be Client or Contributor</param>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost("Register/{role}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<string>> Register([FromRoute] string role, [FromBody] UserRegisterModel register)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest(string.Format(IsRequired, "Role"));
            }

            try
            {
                AppUser user = new(register.Username, register.Email, register.FirstName, register.LastName);
                IdentityResult result = await userManager.CreateAsync(user, register.Password).ConfigureAwait(false);

                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                if (!(role == "Client" || role == "Contributor"))
                {
                    return BadRequest(ForbiddenRoleRegister);
                }

                await userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
                await user.SignInAsync(signInManager, GetAuthProps(false)).ConfigureAwait(false);

                Response.Cookies.Append("role", role);
                Response.Cookies.Append("username", user.UserName!);

                return "Welcome!";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex.GetMessage());
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.GetMessage());
            }
            catch (ArgumentNullException)
            {
                return NotFound(UserHasNoRole);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(UserHasRoles);
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        /// <summary>
        ///     Logs into the account with the specified parameters.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [Consumes("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType(Status423Locked)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginModel model)
        {
            try
            {
                AppUser? user = await userManager.FindByNameAsync(model.Username).ConfigureAwait(false);

                if (user == null)
                {
                    return Unauthorized(InvalidLogin);
                }

                if (await userManager.IsLockedOutAsync(user).ConfigureAwait(false) && user.LockoutEnd.HasValue)
                {
                    DateTimeOffset lockoutEnd = user.LockoutEnd.Value;
                    TimeSpan timeLeft = lockoutEnd.Subtract(DateTimeOffset.UtcNow);

                    return StatusCode(Status423Locked, new
                    {
                        Seconds = Convert.ToInt16(timeLeft.TotalSeconds),
                        Error = string.Format(LockedOutUser, timeLeft.Seconds)
                    });
                }

                SignInResult result = await signInManager.PasswordSignInAsync(
                    user, 
                    model.Password, 
                    model.RememberMe, 
                    lockoutOnFailure: true)
                    .ConfigureAwait(false);

                if (result.Succeeded)
                {
                    await user.SignInAsync(signInManager, GetAuthProps(model.RememberMe)).ConfigureAwait(false);

                    var roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
                    Response.Cookies.Append("role", roles.Single());
                    Response.Cookies.Append("username", user.UserName!);

                    return "Welcome back!";
                }
                else
                {
                    if (result.IsLockedOut && user.LockoutEnd.HasValue)
                    {
                        DateTimeOffset lockoutEnd = user.LockoutEnd.Value;
                        TimeSpan timeLeft = lockoutEnd.Subtract(DateTimeOffset.UtcNow);
                        
                        return StatusCode(Status423Locked, string.Format(LockedOutUser, timeLeft.TotalSeconds));
                    }
                    
                    return Unauthorized(InvalidLogin);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        /// <summary>
        ///     Logs out of the current account.
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<string>> Logout()
        {
            try
            {
                await signInManager.Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
                await signInManager.SignOutAsync().ConfigureAwait(false);

                Response.Cookies.Delete("username");
                Response.Cookies.Delete("role");
                
                return "Bye-bye.";
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        /// <summary>
        ///     Gets info about User Authentication.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Authentication")]
        [ProducesResponseType(Status200OK)]
        public ActionResult<bool> IsAuthenticated() => User.Identity?.IsAuthenticated ?? false;

        /// <summary>
        ///     Gets info about User Authorization.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Authorization")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType (Status500InternalServerError)]
        public async Task<ActionResult<string>> GetUserRole()
        {
            try
            {
                AppUser? user = await userManager.GetUserAsync(User).ConfigureAwait(false);
                if (user == null)
                {
                    return Unauthorized(UnauthenticatedUser);
                }

                IEnumerable<string> roles = await userManager.GetRolesAsync(user);
                return roles.Single();
            }
            catch (ArgumentNullException)
            {
                return StatusCode(Status500InternalServerError, UserHasNoRole);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(Status500InternalServerError, UserHasRoles);
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
