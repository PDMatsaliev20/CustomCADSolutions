using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Identity;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using CustomCADs.Infrastructure.Identity;
using CustomCADs.Infrastructure.Identity.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using static CustomCADs.API.Helpers.Utilities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CustomCADs.API.Controllers
{
    using static ApiMessages;
    using static StatusCodes;

    /// <summary>
    ///     Controller for managing Authentication/Authorization and info about the User's Identity.
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="appUserManager"></param>
    /// <param name="appSignInManager"></param>
    /// <param name="config"></param>
    [ApiController]
    [Route("API/[controller]")]
    public class IdentityController(IUserService userService, IAppUserManager appUserManager, SignInManager<AppUser> appSignInManager, IConfiguration config) : ControllerBase
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
                AppUser user = new(register.Username, register.Email);
                IdentityResult result = await appUserManager.CreateAsync(user, register.Password).ConfigureAwait(false);

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

                await appUserManager.AddToRoleAsync(user, role).ConfigureAwait(false);
                await appSignInManager.SignInAsync(user, false).ConfigureAwait(false);

                Response.Cookies.Append("role", role);
                Response.Cookies.Append("username", user.UserName!);

                UserModel model = new()
                {
                    UserName = register.Username,
                    Email = register.Email,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    RoleName = role,
                };
                string id = await userService.CreateAsync(model).ConfigureAwait(false);

                JwtSecurityToken jwt = config.GenerateAccessToken(id, model.UserName, model.RoleName);
                string signedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                Response.Cookies.Append("jwt", signedJwt, new() { HttpOnly = true, Secure = true, Expires = jwt.ValidTo });                

                (string newRT, DateTime newEnd) = await userService.RenewRefreshToken(model).ConfigureAwait(false);
                Response.Cookies.Append("rt", newRT, new() { HttpOnly = true, Secure = true, Expires = newEnd });

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
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [Consumes("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType(Status423Locked)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginModel login)
        {
            try
            {
                AppUser? user = await appUserManager.FindByNameAsync(login.Username).ConfigureAwait(false);
                if (user == null)
                {
                    return Unauthorized(InvalidLogin);
                }

                if (await appUserManager.IsLockedOutAsync(user).ConfigureAwait(false) && user.LockoutEnd.HasValue)
                {
                    TimeSpan timeLeft = user.LockoutEnd.Value.Subtract(DateTimeOffset.UtcNow);

                    return StatusCode(Status423Locked, new
                    {
                        Seconds = Convert.ToInt16(timeLeft.TotalSeconds),
                        Error = string.Format(LockedOutUser, timeLeft.Seconds)
                    });
                }

                SignInResult result = await appSignInManager.PasswordSignInAsync(
                    user,
                    login.Password,
                    login.RememberMe,
                    lockoutOnFailure: true)
                    .ConfigureAwait(false);

                if (result.Succeeded)
                {
                    UserModel model = await userService.GetByName(login.Username).ConfigureAwait(false);
                    
                    Response.Cookies.Append("role", model.RoleName);
                    Response.Cookies.Append("username", model.UserName);

                    JwtSecurityToken jwt = config.GenerateAccessToken(model.Id, model.UserName, model.RoleName);
                    string signedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                    Response.Cookies.Append("jwt", signedJwt, new() { HttpOnly = true, Secure = true, Expires = jwt.ValidTo });

                    (string newRT, DateTime newEnd) = await userService.RenewRefreshToken(model).ConfigureAwait(false);
                    Response.Cookies.Append("rt", newRT, new() { HttpOnly = true, Secure = true, Expires = newEnd });

                    return "Welcome back!";
                }
                else
                {
                    if (result.IsLockedOut && user.LockoutEnd.HasValue)
                    {
                        TimeSpan timeLeft = user.LockoutEnd.Value.Subtract(DateTimeOffset.UtcNow);

                        return StatusCode(Status423Locked, new
                        {
                            Seconds = Convert.ToInt16(timeLeft.TotalSeconds),
                            Error = string.Format(LockedOutUser, timeLeft.Seconds)
                        });
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
                UserModel model = await userService.GetByIdAsync(User.GetId()).ConfigureAwait(false);
                await appSignInManager.SignOutAsync().ConfigureAwait(false);

                model.RefreshToken = null;
                model.RefreshTokenEndDate = null;
                await userService.EditAsync(model.UserName, model);

                Response.Cookies.Delete("jwt");
                Response.Cookies.Delete("rt");
                Response.Cookies.Delete("username");
                Response.Cookies.Delete("role");

                return "Bye-bye.";
            }
            catch (ArgumentNullException)
            {
                return BadRequest(AlreadyLoggedOut);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        /// <summary>
        ///     Returns a Refresh token
        /// </summary>
        /// <returns></returns>
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            string? rt = Request.Cookies.FirstOrDefault(c => c.Key == "rt").Value;
            if (string.IsNullOrEmpty(rt))
            {
                return BadRequest(NoRefreshToken);
            }

            UserModel model = await userService.GetByRefreshToken(rt).ConfigureAwait(false);
            if (model.RefreshTokenEndDate < DateTime.UtcNow)
            {
                return StatusCode(Status401Unauthorized);
            }

            JwtSecurityToken newJwt = config.GenerateAccessToken(model.Id, model.UserName, model.RoleName);
            string signedJwt = new JwtSecurityTokenHandler().WriteToken(newJwt);
            Response.Cookies.Append("jwt", signedJwt, new() { HttpOnly = true, Secure = true, Expires = newJwt.ValidTo });

            if (model.RefreshTokenEndDate >= DateTime.UtcNow.AddMinutes(1))
            {
                return NoNeedForNewRT;
            }

            (string newRT, DateTime newEnd) = await userService.RenewRefreshToken(model).ConfigureAwait(false);
            Response.Cookies.Append("rt", newRT, new() { HttpOnly = true, Secure = true, Expires = newEnd });

            return AccessTokenRenewed;
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
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<string>> GetUserRole()
        {
            try
            {
                UserModel model = await userService.GetByIdAsync(User.GetId());
                return model.RoleName;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
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
