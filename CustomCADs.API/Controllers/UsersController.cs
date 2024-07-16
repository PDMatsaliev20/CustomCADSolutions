using AutoMapper;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Others;
using CustomCADs.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;

    [ApiController]
    [Route("API/[controller]")]
    [Authorize("Administrator")]
    public class UsersController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : ControllerBase
    {
        private readonly IMapper mapper = new MapperConfiguration(opt
                => opt.AddProfile<OtherApiProfile>())
            .CreateMapper();

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        public async Task<ActionResult<UserDTO[]>> GetAsync()
        {
            AppUser[] users = await userManager.Users.ToArrayAsync();
            UserDTO[] dtos = mapper.Map<UserDTO[]>(users);
            
            for (int i = 0; i < dtos.Length; i++)
            {
                IList<string> roles = await userManager.GetRolesAsync(users[i]);
                dtos[i].Role = roles.Single();
            }
            return dtos;
        }


        [HttpGet("{name}")]
        public async Task<ActionResult<UserDTO>> GetSingleAsync(string name)
        {
            AppUser? user = await userManager.FindByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }
            IList<string> roles = await userManager.GetRolesAsync(user);

            UserDTO dto = mapper.Map<UserDTO>(user);
            dto.Role = roles.Single();
            return dto;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> PostAsync(string username)
        {
            AppUser user = new(username) { Email = $"{username}@gmail.com" };
            IdentityResult result = await userManager.CreateAsync(user);
            if (!result.Succeeded) 
            {
                return StatusCode(Status500InternalServerError, result.Errors);
            }
            
            await userManager.AddToRoleAsync(user, Client);

            return Ok($"Successful creation of client {username}");
        }

        [HttpPatch("{username}/{newRole}")]
        public async Task<ActionResult> PatchAsync(string username, string newRole)
        {
            if (!await roleManager.RoleExistsAsync(newRole))
            {
                return BadRequest($"No such role exists - pick from [{string.Join(", ", roleManager.Roles)}]");
            }

            try
            {
                AppUser? user = await userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return NotFound();
                }
                IList<string> roles = await userManager.GetRolesAsync(user);

                await userManager.RemoveFromRoleAsync(user, roles.Single());
                await userManager.AddToRoleAsync(user, newRole);

                return Ok($"Successfuly changed {user.UserName}'s role from {roles.Single()} to {newRole}.");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"{e.GetType()}: {e.Message}");
            }
        }
    }
}
