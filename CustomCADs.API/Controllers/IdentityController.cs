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
    /// <param name="emailService"></param>
    /// <param name="config"></param>
    /// <param name="env"></param>
    [ApiController]
    [Route("API/[controller]")]
    public class IdentityController(IUserService userService, IAppUserManager appUserManager, SignInManager<AppUser> appSignInManager, IEmailService emailService, IConfiguration config, IHostEnvironment env) : ControllerBase
    {
        private readonly string serverPath = env.EnvironmentName switch
        {
            "Production" => "https://customcads.onrender.com",
            "Development" => "https://localhost:7127",
            _ => "",
        } + "/API/Identity";

        private readonly string clientPath = env.EnvironmentName switch
        {
            "Production" => "https://customcads.onrender.com",
            "Development" => "https://localhost:5173",
            _ => "",
        };

        /// <summary>
        ///     Creates a new account with the specified parameters for the user and verifies the ownership of the email by sending a token.
        /// </summary>
        /// <param name="role">Must be Client or Contributor</param>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost("Register/{role}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status409Conflict)]
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

                UserModel model = new()
                {
                    UserName = register.Username,
                    Email = string.Empty,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    RoleName = role,
                };
                await userService.CreateAsync(model).ConfigureAwait(false);

                string ect = await appUserManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                string endpoint = serverPath + $"/VerifyEmail/{model.UserName}?ect={ect}";

                await emailService.SendVerificationEmailAsync(register.Email, endpoint).ConfigureAwait(false);
                return "Check your email.";
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
        ///     Checks the token's validity, and if successful verifies the user's email and  logs the him in.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="ect"></param>
        /// <returns></returns>
        [HttpGet("VerifyEmail/{username}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<string>> ConfirmEmailAsync(string username, string ect)
        {
            try
            {
                AppUser? appUser = await appUserManager.FindByNameAsync(username).ConfigureAwait(false);
                if (appUser == null)
                {
                    return NotFound(string.Format(ApiMessages.NotFound, "Account"));
                }

                if (appUser.EmailConfirmed)
                {
                    return BadRequest(EmailAlreadyVerified);
                }

                string decodedECT = ect.Replace(' ', '+');
                IdentityResult result = await appUserManager.ConfirmEmailAsync(appUser, decodedECT).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return BadRequest(InvalidEmailToken);
                }

                await appSignInManager.SignInAsync(appUser, false).ConfigureAwait(false);
                UserModel model = await userService.GetByName(username).ConfigureAwait(false);

                Response.Cookies.Append("role", model.RoleName);
                Response.Cookies.Append("username", model.UserName);

                JwtSecurityToken jwt = config.GenerateAccessToken(model.Id, model.UserName, model.RoleName);
                string signedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                Response.Cookies.Append("jwt", signedJwt, new() { HttpOnly = true, Secure = true, Expires = jwt.ValidTo });

                (string newRT, DateTime newEnd) = await userService.RenewRefreshToken(model).ConfigureAwait(false);
                Response.Cookies.Append("rt", newRT, new() { HttpOnly = true, Secure = true, Expires = newEnd });

                Response.Cookies.Append("role", model.RoleName, new() { Expires = newEnd });
                Response.Cookies.Append("username", model.UserName, new() { Expires = newEnd });

                return "Welcome!";
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        /// <summary>
        ///     Sends another email with a token.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("ResendEmailVerification")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<string>> ResendEmailVerificationAsync(string username)
        {
            try
            {
                AppUser? user = await appUserManager.FindByNameAsync(username).ConfigureAwait(false);
                if (user == null)
                {
                    return NotFound(string.Format(ApiMessages.NotFound, "Account"));
                }

                if (user.EmailConfirmed)
                {
                    return BadRequest(EmailAlreadyVerified);
                }

                string ect = await appUserManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                string endpoint = serverPath + $"/VerifyEmail/{username}?ect={ect}";

                await emailService.SendVerificationEmailAsync(user.Email!, endpoint).ConfigureAwait(false);
                return "Check your email.";
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
                if (user == null || !user.EmailConfirmed)
                {
                    return Unauthorized(InvalidAccountOrEmail);
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

                    JwtSecurityToken jwt = config.GenerateAccessToken(model.Id, model.UserName, model.RoleName);
                    string signedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                    Response.Cookies.Append("jwt", signedJwt, new() { HttpOnly = true, Secure = true, Expires = jwt.ValidTo });

                    (string newRT, DateTime newEnd) = await userService.RenewRefreshToken(model).ConfigureAwait(false);
                    Response.Cookies.Append("rt", newRT, new() { HttpOnly = true, Secure = true, Expires = newEnd });

                    Response.Cookies.Append("role", model.RoleName, new() { Expires = newEnd });
                    Response.Cookies.Append("username", model.UserName, new() { Expires = newEnd });

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

        [HttpPost("ResetPassword/{email}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<string>> ResetPassword(string email, string token, string newPassword)
        {
            try
            {
                AppUser? user = await appUserManager.FindByEmailAsync(email).ConfigureAwait(false);
                if (user == null)
                {
                    return NotFound(string.Format(ApiMessages.NotFound, "User"));
                }

                string encodedToken = token.Replace(' ', '+');
                IdentityResult result = await appUserManager.ResetPasswordAsync(user, encodedToken, newPassword).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                return "Done!";
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpGet("ForgotPassword/{email}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<string>> ForgotPassword(string email)
        {
            try
            {
                AppUser? user = await appUserManager.FindByEmailAsync(email).ConfigureAwait(false);
                if (user == null)
                {
                    return NotFound(string.Format(ApiMessages.NotFound, "User"));
                }

                string token = await appUserManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);

                string endpoint = clientPath + $"/login/reset-password?email={email}&token={token}";
                await emailService.SendForgotPasswordEmailAsync(email, endpoint).ConfigureAwait(false);

                return "Check your email!";
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
            try
            {
                string? rt = Request.Cookies.FirstOrDefault(c => c.Key == "rt").Value;
                if (string.IsNullOrEmpty(rt))
                {
                    return BadRequest(NoRefreshToken);
                }

                UserModel model = await userService.GetByRefreshToken(rt).ConfigureAwait(false);
                if (model.RefreshTokenEndDate < DateTime.UtcNow)
                {
                    Response.Cookies.Delete("rt");
                    Response.Cookies.Delete("username");
                    Response.Cookies.Delete("userRole");
                    return StatusCode(Status401Unauthorized, RefreshTokenExpired);
                }

                JwtSecurityToken newJwt = config.GenerateAccessToken(model.Id, model.UserName, model.RoleName);
                string signedJwt = new JwtSecurityTokenHandler().WriteToken(newJwt);
                Response.Cookies.Append("jwt", signedJwt, new() { HttpOnly = true, Secure = true, Expires = newJwt.ValidTo });

                if (model.RefreshTokenEndDate >= DateTime.UtcNow.AddMinutes(1))
                {
                    return Ok(NoNeedForNewRT);
                }

                (string newRT, DateTime newEnd) = await userService.RenewRefreshToken(model).ConfigureAwait(false);
                Response.Cookies.Append("rt", newRT, new() { HttpOnly = true, Secure = true, Expires = newEnd });

                Response.Cookies.Append("role", model.RoleName, new() { Expires = newEnd });
                Response.Cookies.Append("username", model.UserName, new() { Expires = newEnd });

                return Ok(AccessTokenRenewed);
            }
            catch (ArgumentNullException)
            {
                Response.Cookies.Delete("rt");
                Response.Cookies.Delete("username");
                Response.Cookies.Delete("userRole");
                return NotFound("Your session has ended, you must log in again.");
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

        /// <summary>
        ///     Gets info about User Email Status
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("IsEmailConfirmed/{username}")]
        [ProducesResponseType(Status200OK)]
        public async Task<bool> IsEmailConfirmed(string username)
        {
            AppUser? user = await appUserManager.FindByNameAsync(username).ConfigureAwait(false);
            return user?.EmailConfirmed ?? false;
        }

        /// <summary>
        ///     Gets info about User Existence Status
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("DoesUserExist/{username}")]
        [ProducesResponseType(Status200OK)]
        public async Task<bool> DoesUserExist(string username)
        {
            AppUser? user = await appUserManager.FindByNameAsync(username).ConfigureAwait(false);
            return user != null;
        }
    }
}
