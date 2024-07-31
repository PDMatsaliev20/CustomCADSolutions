using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Users;
using CustomCADs.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static CustomCADs.API.Helpers.Utilities;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;

    [ApiController]
    [Route("API/[controller]")]
    public class IdentityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : ControllerBase
    {
        [HttpPost("Register/{role}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<string>> Register([FromRoute] string role, [FromBody] UserRegisterModel register)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return BadRequest("Role is required!");
            }

            try
            {
                AppUser user = new()
                {
                    UserName = register.Username,
                    Email = register.Email
                };
                IdentityResult result = await userManager.CreateAsync(user, register.Password);

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
                    return BadRequest("You must apply to either be a Client or a Contributor");
                }

                await userManager.AddToRoleAsync(user, role);
                await user.SignInAsync(signInManager, GetAuthProps(false));

                Response.Cookies.Append("username", user.UserName);
                return "Welcome!";
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpPost("Login")]
        [Consumes("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginModel model)
        {
            try
            {
                AppUser? user = await userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    return BadRequest("Invalid Username.");
                }

                if (!await userManager.CheckPasswordAsync(user, model.Password))
                {
                    return Unauthorized("Invalid Password.");
                }
                await user.SignInAsync(signInManager, GetAuthProps(model.RememberMe));

                Response.Cookies.Append("username", user.UserName!);
                return "Welcome back!";
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpPost("Logout")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<string>> Logout()
        {
            try
            {
                await signInManager.Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                Response.Cookies.Delete("username");
                return "Bye-bye.";
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpGet("IsAuthenticated")]
        [ProducesResponseType(Status200OK)]
        public ActionResult<bool> IsAuthenticated() => User.Identity?.IsAuthenticated ?? false;

        [HttpGet("GetUserRole")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status401Unauthorized)]
        [ProducesResponseType (Status500InternalServerError)]
        public async Task<ActionResult<string>> GetUserRole()
        {
            try
            {
                AppUser? user = await userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                IEnumerable<string> roles = await userManager.GetRolesAsync(user);
                return roles.Single();
            }
            catch (ArgumentNullException)
            {
                return StatusCode(Status500InternalServerError, "User has no role.");
            }
            catch (InvalidOperationException)
            {
                return StatusCode(Status500InternalServerError, "User has more than one role.");
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
