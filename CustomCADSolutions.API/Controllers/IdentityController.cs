using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using CustomCADSolutions.API.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CustomCADSolutions.API.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class IdentityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config) : ControllerBase
    {
        private readonly AuthenticationProperties authProps = new()
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
        };
        private readonly AuthenticationProperties rememberMeAuthProp = new()
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
        };

        [Authorize]
        [HttpGet("IsAuthenticated")]
        public ActionResult Auth() => Ok("You're authenticated!");

        [HttpPost("Register/{role}")]
        [Consumes("application/json")]
        public async Task<ActionResult> Register([FromRoute] string role, [FromBody] UserRegisterModel model)
        {
            AppUser user = new()
            {
                UserName = model.Username,
                Email = model.Email
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
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
            await user.SignIn(signInManager, authProps);

            return Ok("Welcome!");
        }

        [HttpPost("Login")]
        [Consumes("application/json")]
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

            await user.SignIn(signInManager,
                model.RememberMe ? rememberMeAuthProp : authProps);

            return Ok("Welcome back!");
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await signInManager.Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok("Bye-bye.");
            }
            catch
            {
                return BadRequest("You're still here?");
            }
        }
    }
}
