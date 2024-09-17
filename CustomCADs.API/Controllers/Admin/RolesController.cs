using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Roles;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Infrastructure.Identity;
using CustomCADs.Infrastructure.Identity.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Controllers.Admin
{
    using static ApiMessages;
    using static StatusCodes;

    /// <summary>
    ///     Admin Controller for managing Roles.
    /// </summary>
    /// <param name="roleService"></param>
    /// <param name="appRoleManager"></param>
    /// <param name="mapper"></param>
    [ApiController]
    [Route("API/Admin/[controller]")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class RolesController(IRoleService roleService, IAppRoleManager appRoleManager, IMapper mapper) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(GetRoleAsync).Replace("Async", "");

        /// <summary>
        ///     Gets All Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public ActionResult<RoleGetDTO[]> GetRolesAsync()
        {
            try
            {
                RoleResult result = roleService.GetAll();
                return mapper.Map<RoleGetDTO[]>(result.Roles);
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

        /// <summary>
        ///     Gets a Role by the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<RoleGetDTO>> GetRoleAsync(string name)
        {
            try
            {
                RoleModel role = await roleService.GetByNameAsync(name).ConfigureAwait(false);
                return mapper.Map<RoleGetDTO>(role);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ApiMessages.NotFound, "Role"));
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

        /// <summary>
        ///     Creates a Role with the specified name.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult> PostRoleAsync(RolePostDTO post)
        {
            try
            {
                IdentityResult result = await appRoleManager.CreateAsync(new(post.Name)).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return StatusCode(Status500InternalServerError, result.Errors);
                }

                RoleModel model = mapper.Map<RoleModel>(post);
                string id = await roleService.CreateAsync(model).ConfigureAwait(false);

                RoleModel newModel = await roleService.GetByIdAsync(id).ConfigureAwait(false);
                RoleGetDTO dto = mapper.Map<RoleGetDTO>(newModel);
                
                return CreatedAtAction(createdAtReturnAction, new { post.Name }, dto);
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

        /// <summary>
        ///     Updates a Role in the traditional way - with an array of operations.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="patchRole"></param>
        /// <returns></returns>
        [HttpPatch("{name}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PatchRoleAsync(string name, [FromBody] JsonPatchDocument<RoleModel> patchRole)
        {
            string? modifiedForbiddenField = patchRole.CheckForBadChanges("/id");
            if (modifiedForbiddenField != null)
            {
                return BadRequest(string.Format(ForbiddenPatch, modifiedForbiddenField));
            }

            try
            {
                RoleModel model = await roleService.GetByNameAsync(name).ConfigureAwait(false);

                string? error = null;
                patchRole.ApplyTo(model, e => error = e.ErrorMessage);

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                await roleService.EditAsync(name, model);

                if (model.Name != name)
                {
                    AppRole? role = await appRoleManager.FindByNameAsync(name).ConfigureAwait(false);
                    if (role == null) 
                    {
                        return BadRequest(string.Format(ApiMessages.NotFound, "Role"));
                    }

                    role.Name = model.Name;
                    await appRoleManager.UpdateAsync(role).ConfigureAwait(false);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        /// <summary>
        ///     Deletes the Role with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> DeleteRoleAsync(string name)
        {
            try
            {
                AppRole? role = await appRoleManager.FindByNameAsync(name).ConfigureAwait(false);
                if (role == null || !await roleService.ExistsByNameAsync(name).ConfigureAwait(false))
                {
                    return NotFound(string.Format(ApiMessages.NotFound, "Role"));
                }

                await appRoleManager.DeleteAsync(role).ConfigureAwait(false);
                await roleService.DeleteAsync(name).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
