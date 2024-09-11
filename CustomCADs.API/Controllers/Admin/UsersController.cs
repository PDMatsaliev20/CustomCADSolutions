using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Identity;
using CustomCADs.API.Models.Users;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Controllers.Admin
{
    using static StatusCodes;
    using static ApiMessages;

    /// <summary>
    ///     Admin Controller for managing Users.
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="appUserManager"></param>
    /// <param name="appRoleManager"></param>
    /// <param name="mapper"></param>
    [ApiController]
    [Route("API/Admin/[controller]")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class UsersController(IUserService userService, AppUserManager appUserManager, AppRoleManager appRoleManager, IMapper mapper) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(GetUserAsync).Replace("Async", "");

        /// <summary>
        ///     Gets All Users.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sorting"></param>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<UserGetDTO[]>> GetUsersAsync(string? name, string sorting, int limit = 50, int page = 1)
        {
            SearchModel search = new() { Name = name, Sorting = sorting ?? string.Empty };
            PaginationModel pagination = new() { Limit = limit, Page = page };

            try
            {
                UserResult result = await userService.GetAllAsync(search, pagination).ConfigureAwait(false);
                UserGetDTO[] gets = mapper.Map<UserGetDTO[]>(result.Users);
                return gets;
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

        /// <summary>
        ///     Gets a User by the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<UserGetDTO>> GetUserAsync(string name)
        {
            UserResult result = await userService.GetAllAsync(new(), new(), u => u.UserName == name).ConfigureAwait(false);
            UserModel model = result.Users.Single();
            
            try
            {
                UserGetDTO get = mapper.Map<UserGetDTO>(model);
                return get;
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

        /// <summary>
        ///     Creates a User with the specified name and role.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost("{username}")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult> PostUserAsync(UserPostDTO post)
        {
            if (!await appRoleManager.RoleExistsAsync(post.Role))
            {
                return BadRequest(string.Format(InvalidRole, string.Join(", ", appRoleManager.Roles)));
            }

            try
            {
                AppUser user = new(post.Username, post.Email);
                IdentityResult result = await appUserManager.CreateAsync(user).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    return StatusCode(Status500InternalServerError, result.Errors);
                }
                await appUserManager.AddToRoleAsync(user, post.Role).ConfigureAwait(false);

                UserModel model = mapper.Map<UserModel>(post);
                string id = await userService.CreateAsync(model);

                UserModel addedModel = await userService.GetByIdAsync(id).ConfigureAwait(false);
                UserGetDTO get = mapper.Map<UserGetDTO>(addedModel);

                return CreatedAtAction(createdAtReturnAction, new { name = get.Username }, get);
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
        ///     Updates a User in the traditional way - with an array of operations.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="patchUser"></param>
        /// <returns></returns>
        [HttpPatch("{username}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PatchUserAsync(string username, [FromBody] JsonPatchDocument<UserModel> patchUser)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(string.Format(IsRequired, "Username"));
            }

            string? modifiedForbiddenField = patchUser.CheckForBadChanges("/id", "refreshToken", "refreshTokenEndDate");
            if (modifiedForbiddenField != null)
            {
                return BadRequest(string.Format(ForbiddenPatch, modifiedForbiddenField));
            }
            
            try
            {
                UserResult result = await userService.GetAllAsync(new(), new(), u => u.UserName == username).ConfigureAwait(false);
                UserModel? model = result.Users.SingleOrDefault();
                AppUser? user = await appUserManager.FindByNameAsync(username).ConfigureAwait(false);

                if (user == null || model == null)
                {
                    return NotFound(string.Format(ApiMessages.NotFound, "User"));
                }

                string oldRole = model.RoleName;

                string? error = null;
                patchUser.ApplyTo(model, a => error = a.ErrorMessage);
                if (string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                string newRole = model.RoleName;
                if (oldRole != newRole)
                {
                    if (!await appRoleManager.RoleExistsAsync(newRole).ConfigureAwait(false))
                    {
                        return BadRequest(string.Format(InvalidRole, string.Join(", ", appRoleManager.Roles)));
                    }
                    
                    await appUserManager.RemoveFromRoleAsync(user, oldRole).ConfigureAwait(false);
                    await appUserManager.AddToRoleAsync(user, newRole).ConfigureAwait(false);
                }
                await appUserManager.UpdateAsync(user).ConfigureAwait(false);

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

        /// <summary>
        ///     Deletes the User with the specified username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete("{username}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> DeleteUserAsync(string username)
        {
            try
            {
                AppUser? user = await appUserManager.FindByNameAsync(username).ConfigureAwait(false);
                if (user == null) 
                {
                    return NotFound(string.Format(ApiMessages.NotFound, "User"));
                }

                await appUserManager.DeleteAsync(user).ConfigureAwait(false);
                await userService.DeleteAsync(username).ConfigureAwait(false);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
