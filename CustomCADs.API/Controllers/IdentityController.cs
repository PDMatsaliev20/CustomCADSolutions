using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Users;
using CustomCADs.Infrastructure.Data.Models.Identity;
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
        public async Task<ActionResult> Register([FromRoute] string role, [FromBody] UserRegisterModel model)
        {
            AppUser user = new()
            {
                UserName = model.Username,
                Email = model.Email
            };
            IdentityResult result = await userManager.CreateAsync(user, model.Password);

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
                return BadRequest();
            }
            await userManager.AddToRoleAsync(user, role);
            await user.SignInAsync(signInManager, GetAuthProps(false));

            Response.Cookies.Append("username", user.UserName);
            return Ok("Welcome!");
        }

        [HttpPost("Login")]
        [Consumes("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] UserLoginModel model)
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
            return Ok("Welcome back!");
        }

        [HttpPost("Logout")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await signInManager.Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                Response.Cookies.Delete("username");
                return Ok("Bye-bye.");
            }
            catch
            {
                return BadRequest("You're still here?");
            }
        }

        [HttpGet("IsAuthenticated")]
        [ProducesResponseType(Status200OK)]
        public ActionResult<bool> IsAuthenticated() => User.Identity?.IsAuthenticated ?? false;

        [HttpGet("IsAuthorized")]
        [ProducesResponseType(Status200OK)]
        public ActionResult<bool> IsAuthorized(string role) => User.IsInRole(role);

        [HttpGet("UserExists")]
        [ProducesResponseType(Status200OK)]
        public ActionResult<bool> Exists() => userManager.Users.Any(u => User.Identity!.Name == u.UserName);

        [HttpGet("GetUserRole")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult> GetUserRole()
        {
            AppUser? user = await userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            IEnumerable<string> roles = await userManager.GetRolesAsync(user);
            try
            {
                string role = roles.Single();

                return Ok(role);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
