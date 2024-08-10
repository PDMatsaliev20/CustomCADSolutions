﻿using AutoMapper;
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
    using static ApiMessages;

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
                AppUser[] users = await userManager.Users.ToArrayAsync().ConfigureAwait(false);

                ICollection<UserDTO> dtos = [];
                foreach (AppUser user in users)
                {
                    UserDTO dto = mapper.Map<UserDTO>(user);
                    IList<string> roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
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

        [HttpGet("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<UserDTO>> GetUserAsync(string name)
        {
            AppUser? user = await userManager.FindByNameAsync(name).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound(string.Format(ApiMessages.NotFound, "User"));
            }

            try
            {
                UserDTO dto = mapper.Map<UserDTO>(user);

                IList<string> roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
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

        [HttpPost("{username}")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult> PostUserAsync(string username, string role)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(string.Format(IsRequired, "Username"));
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                return BadRequest(string.Format(InvalidRole, string.Join(", ", roleManager.Roles)));
            }

            try
            {
                AppUser user = new(username) { Email = $"{username}@gmail.com" };
                IdentityResult result = await userManager.CreateAsync(user).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return StatusCode(Status500InternalServerError, result.Errors);
                }

                await userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
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
                return BadRequest(string.Format(IsRequired, "Username"));
            }

            string? modifiedForbiddenField = patchUser.CheckForBadChanges("/id", "/accessFailedCount", "/emailConfirmed", "/phoneNumberConfirmed", "/normalizedName", "/normalizedEmail", "/concurrencyStamp", "/securityStamp", "/lockoutEnabled", "/lockoutEnd", "/passwordHash", "/twoFactorEnabled");
            if (modifiedForbiddenField != null)
            {
                return BadRequest(string.Format(ForbiddenPatch, modifiedForbiddenField));
            }
            
            try
            {
                AppUser? user = await userManager.FindByNameAsync(username).ConfigureAwait(false);
                if (user == null)
                {
                    return NotFound(string.Format(ApiMessages.NotFound, "User"));
                }

                if (!string.IsNullOrWhiteSpace(newRole))
                {
                    if (!await roleManager.RoleExistsAsync(newRole).ConfigureAwait(false))
                    {
                        return BadRequest(string.Format(InvalidRole, string.Join(", ", roleManager.Roles)));
                    }
                    IList<string> roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);

                    await userManager.RemoveFromRoleAsync(user, roles.Single()).ConfigureAwait(false);
                    await userManager.AddToRoleAsync(user, newRole).ConfigureAwait(false);
                }

                string? error = null;
                patchUser.ApplyTo(user, a => error = a.ErrorMessage);

                if (string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }
                await userManager.UpdateAsync(user).ConfigureAwait(false);

                return NoContent();
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

        [HttpDelete("{username}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> DeleteUserAsync(string username)
        {
            try
            {
                AppUser? user = await userManager.FindByNameAsync(username).ConfigureAwait(false);
                if (user == null) 
                {
                    return NotFound(string.Format(ApiMessages.NotFound, "User"));
                }

                await userManager.DeleteAsync(user).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
