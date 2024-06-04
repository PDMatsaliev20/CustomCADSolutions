using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CustomCADSolutions.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CustomCADSolutions.API.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class IdentityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config) : ControllerBase
    {
        [HttpGet("IsAuthenticated")]
        public ActionResult<bool> IsAuthenticated()
        {
            KeyValuePair<string, string>? nullableCookie = Request.Cookies.FirstOrDefault(c => c.Key == "token");

            if (nullableCookie == null || !nullableCookie.HasValue)
            {
                return Ok(new { isAuthenticated = false });
            }
            KeyValuePair<string, string> cookie = nullableCookie.Value;

            string jwt = cookie.Value;
            string key = config["JwtSettings:SecretKey"] ?? throw new Exception("SecretKey not provided.");
            string issuer = config["JwtSettings:Issuer"] ?? throw new Exception("Issuer not provided.");
            string audience = config["JwtSettings:Audience"] ?? throw new Exception("Audience not provided.");
            bool tokenIsValid = JwtExtensions.IsTokenValid(jwt, key, issuer, audience);

            return Ok(new { isAuthenticated = tokenIsValid });
        }
        

        [HttpPost("Register/{role}")]
        [Consumes("application/json")]
        public async Task<ActionResult> Register([FromRoute] string role, [FromBody] UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
            await signInManager.SignInAsync(user, isPersistent: false);

            string token = await GenerateJwtTokenAsync(user);
            return Ok(new { token, role, username = user.UserName});
        }

        [HttpPost("Login")]
        [Consumes("application/json")]
        public async Task<ActionResult> Login([FromBody] UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest();
            }

            AppUser user = (await userManager.FindByNameAsync(model.Username))!;

            string token = await GenerateJwtTokenAsync(user);
            string role = (await userManager.GetRolesAsync(user)).Single();

            return Ok(new { token, role, username = user.UserName });
        }

        [HttpPost("Logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await signInManager.SignOutAsync();
                return Ok("Bye-bye.");
            }
            catch
            {
                return BadRequest("You're still here?");
        }
        }
    }
}
