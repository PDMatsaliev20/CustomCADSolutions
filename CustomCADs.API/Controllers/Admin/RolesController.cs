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
    public class RolesController(RoleManager<AppRole> roleManager) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(GetRoleAsync).Replace("Async", "");
        private readonly IMapper mapper = new MapperConfiguration(opt
                => opt.AddProfile<IdentityApiProfile>())
            .CreateMapper();

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<RoleDTO[]>> GetRolesAsync()
        {
            try
            {
                AppRole[] roles = await roleManager.Roles.ToArrayAsync();
                return mapper.Map<RoleDTO[]>(roles);
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

        [HttpGet("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<RoleDTO>> GetRoleAsync(string name)
        {
            try
            {
                AppRole? role = await roleManager.FindByNameAsync(name);

                return role == null ? NotFound("Role not found.") : mapper.Map<RoleDTO>(role);
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

        [HttpPost("{name}")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult> PostRoleAsync(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name can't empty.");
            }
            
            try
            {
                AppRole role = new(name, description);
                IdentityResult result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    return StatusCode(Status500InternalServerError, result.Errors);
                }

                RoleDTO dto = mapper.Map<RoleDTO>(role);
                return CreatedAtAction(createdAtReturnAction, new { name }, dto);
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

        [HttpPatch("{name}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PatchUserAsync(string name, [FromBody] JsonPatchDocument<AppRole> patchRole)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name can't empty.");
            }

            string? modifiedForbiddenField = patchRole.CheckForBadChanges("/id", "/normalizedName", "/concurrencyStamp");
            if (modifiedForbiddenField != null)
            {
                return BadRequest($"You're not allowed to edit {modifiedForbiddenField}");
            }

            try
            {
                AppRole? role = await roleManager.FindByNameAsync(name);
                if (role == null)
                {
                    return NotFound("Role not found.");
                }

                string? error = null;
                patchRole.ApplyTo(role, e => error = e.ErrorMessage);

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }
                await roleManager.UpdateAsync(role);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> DeleteRoleAsync(string name)
        {
            try
            {
                AppRole? role = await roleManager.FindByNameAsync(name);
                if (role == null)
                {
                    return NotFound("Role not found.");
                }

                await roleManager.DeleteAsync(role);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
