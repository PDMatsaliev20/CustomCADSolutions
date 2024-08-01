using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Others;
using CustomCADs.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Controllers.Admin
{
    using static StatusCodes;

    [ApiController]
    [Route("API/Admin/[controller]")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class UsersController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(GetUserAsync).Replace("Async", "");
        private readonly IMapper mapper = new MapperConfiguration(opt
                => opt.AddProfile<IdentityApiProfile>())
            .CreateMapper();

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<UserDTO[]>> GetUsersAsync()
        {
            try
            {
                AppUser[] users = await userManager.Users.ToArrayAsync();

                ICollection<UserDTO> dtos = [];
                foreach (AppUser user in users)
                {
                    UserDTO dto = mapper.Map<UserDTO>(user);
                    IList<string> roles = await userManager.GetRolesAsync(user);
                    dto.Role = roles.Single();
                    dtos.Add(dto);
                }

                return dtos.ToArray();
            }
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
            }
            catch (ArgumentNullException)
            {
                return NotFound("User has no role.");
            }
            catch (InvalidOperationException)
            {
                return BadRequest("User has more than one role.");
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpGet("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<UserDTO>> GetUserAsync(string name)
        {
            AppUser? user = await userManager.FindByNameAsync(name);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            try
            {
                UserDTO dto = mapper.Map<UserDTO>(user);

                IList<string> roles = await userManager.GetRolesAsync(user);
                dto.Role = roles.Single();

                return dto;
            }
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
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

        [HttpPost("{username}")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult> PostUserAsync(string username, string role)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username can't be empty.");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                return BadRequest($"You must choose a role from [{string.Join(", ", roleManager.Roles)}]");
            }

            try
            {
                AppUser user = new(username) { Email = $"{username}@gmail.com" };
                IdentityResult result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return StatusCode(Status500InternalServerError, result.Errors);
                }

                await userManager.AddToRoleAsync(user, role);
                UserDTO dto = mapper.Map<UserDTO>(user);

                return CreatedAtAction(createdAtReturnAction, new { name = username }, dto);
            }
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpPatch("{username}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PatchUserAsync(string username, [FromBody] JsonPatchDocument<AppUser> patchUser, string? newRole)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username can't empty.");
            }

            string? modifiedForbiddenField = patchUser.CheckForBadChanges("/id", "/accessFailedCount", "/emailConfirmed", "/phoneNumberConfirmed", "/normalizedName", "/normalizedEmail", "/concurrencyStamp", "/securityStamp", "/lockoutEnabled", "/lockoutEnd", "/passwordHash", "/twoFactorEnabled");
            if (modifiedForbiddenField != null)
            {
                return BadRequest($"You're not allowed to edit {modifiedForbiddenField}");
            }
            
            try
            {
                AppUser? user = await userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                if (!string.IsNullOrWhiteSpace(newRole))
                {
                    if (!await roleManager.RoleExistsAsync(newRole))
                    {
                        return BadRequest($"You must choose a role from [{string.Join(", ", roleManager.Roles)}]");
                    }
                    IList<string> roles = await userManager.GetRolesAsync(user);

                    await userManager.RemoveFromRoleAsync(user, roles.Single());
                    await userManager.AddToRoleAsync(user, newRole);
                }

                string? error = null;
                patchUser.ApplyTo(user, a => error = a.ErrorMessage);

                if (string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }
                await userManager.UpdateAsync(user);

                return NoContent();
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

        [HttpDelete("{username}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> DeleteUserAsync(string username)
        {
            try
            {
                AppUser? user = await userManager.FindByNameAsync(username);
                if (user == null) 
                {
                    return NotFound("User not found.");
                }

                await userManager.DeleteAsync(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
