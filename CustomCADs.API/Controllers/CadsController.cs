using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;
    using static ApiMessages;

    [Authorize(Roles = $"{Contributor},{Designer}")]
    [ApiController]
    [Route("API/[controller]")]
    public class CadsController(ICadService cadService, IWebHostEnvironment env, IMapper mapper) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(GetCadAsync).Replace("Async", "");
        
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadQueryResultDTO>> GetCadsAsync([FromQuery] PaginationModel pagination, string? sorting, string? category, string? name)
        {
            CadQuery query = new() { Creator = User.Identity!.Name };
            SearchModel search = new() { Category = category, Name = name, Sorting = sorting ?? "" };

            try
            {
                CadResult result = await cadService.GetAllAsync(query, search, pagination).ConfigureAwait(false);
                return mapper.Map<CadQueryResultDTO>(result);
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
        
        [HttpGet("Recents")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadQueryResultDTO>> GetRecentCadsAsync()
        {
            CadQuery query = new() { Creator = User.Identity!.Name };
            SearchModel search = new() { Sorting = nameof(Sorting.Newest) };
            PaginationModel pagination = new() { Limit = 4 };

            try
            {
                CadResult result = await cadService.GetAllAsync(query, search, pagination).ConfigureAwait(false);
                return mapper.Map<CadQueryResultDTO>(result);
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
        
        [HttpGet("Counts")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public ActionResult<dynamic> GetCadsCountsAsync()
        {
            try
            {
                bool predicate(CadModel cad, CadStatus s)
                    => cad.Status == s && cad.Creator.UserName == User.Identity!.Name;

                int uncheckedCadsCounts = cadService.Count(c => predicate(c, CadStatus.Unchecked));
                int validatedCadsCounts = cadService.Count(c => predicate(c, CadStatus.Validated));
                int reportedCadsCounts = cadService.Count(c => predicate(c, CadStatus.Reported));
                int bannedCadsCounts = cadService.Count(c => predicate(c, CadStatus.Banned));

                return new { Unchecked = uncheckedCadsCounts, validated = validatedCadsCounts, reported = reportedCadsCounts, banned = bannedCadsCounts };
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadGetDTO>> GetCadAsync(int id)
        {
            try
            {
                CadModel model = await cadService.GetByIdAsync(id).ConfigureAwait(false);

                if (model.Creator.UserName != User.Identity!.Name)
                {
                    return StatusCode(403, ForbiddenAccess);
                }

                return mapper.Map<CadGetDTO>(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ApiMessages.NotFound, "Cad"));
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

        [HttpPost]
        [RequestSizeLimit(300_000_000)]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult> PostCadAsync([FromForm] CadPostDTO import)
        {
            try
            {
                CadModel model = mapper.Map<CadModel>(import);
                model.CreationDate = DateTime.Now;
                model.CreatorId = User.GetId();
                model.Status = User.IsInRole(Designer) ?
                    CadStatus.Validated : CadStatus.Unchecked;

                int id = await cadService.CreateAsync(model).ConfigureAwait(false);
                model = await cadService.GetByIdAsync(id).ConfigureAwait(false);

                string imagePath = await env.UploadImageAsync(import.Image, model.Name + id + import.Image.GetFileExtension()).ConfigureAwait(false);
                string cadPath = await env.UploadCadAsync(import.File, model.Name + id, import.File.GetFileExtension()).ConfigureAwait(false);
                await cadService.SetPathsAsync(id, cadPath, imagePath).ConfigureAwait(false);

                model = await cadService.GetByIdAsync(id).ConfigureAwait(false);
                CadGetDTO export = mapper.Map<CadGetDTO>(model);
                return CreatedAtAction(createdAtReturnAction, new { id }, export);
            }
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex.GetMessage());
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.GetMessage());
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PutCadAsync(int id, [FromForm] CadPutDTO dto)
        {
            try
            {
                CadModel cad = await cadService.GetByIdAsync(id).ConfigureAwait(false);

                if (User.Identity!.Name != cad.Creator.UserName)
                {
                    return StatusCode(403, ForbiddenAccess);
                }

                if (dto.Image != null)
                {
                    env.DeleteFile("images", cad.Name + id, cad.ImageExtension);
                    string imagePath = await env.UploadImageAsync(dto.Image, dto.Name + id + dto.Image.GetFileExtension()).ConfigureAwait(false);
                    await cadService.SetPathsAsync(id, cad.CadPath, imagePath).ConfigureAwait(false);
                }

                cad.Name = dto.Name;
                cad.Description = dto.Description;
                cad.CategoryId = dto.CategoryId;
                cad.Price = dto.Price;
                await cadService.EditAsync(id, cad).ConfigureAwait(false);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex.GetMessage());
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status409Conflict)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PatchCadAsync(int id, [FromBody] JsonPatchDocument<CadModel> patchCad)
        {
            string? modifiedForbiddenField = patchCad.CheckForBadChanges("/id", "/imagePath", "/cadPath", "/status", "/creationDate", "/creatorId", "/category", "/creator", "/orders");
            if (modifiedForbiddenField != null)
            {
                return BadRequest(string.Format(ForbiddenPatch, modifiedForbiddenField));
            }

            try
            {
                CadModel model = await cadService.GetByIdAsync(id).ConfigureAwait(false);

                if (model.Creator.UserName != User.Identity!.Name)
                {
                    return StatusCode(403, ForbiddenAccess);
                }

                string? error = null;
                patchCad.ApplyTo(model, p => error = p.ErrorMessage);
                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                bool isValid = model.Validate(out IList<string> errors);
                if (!isValid)
                {
                    return BadRequest(string.Join("; ", errors));
                }

                await cadService.EditAsync(id, model).ConfigureAwait(false);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex.GetMessage());
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> DeleteCadAsync(int id)
        {   
            try
            {
                CadModel model = await cadService.GetByIdAsync(id).ConfigureAwait(false);

                if (model.Creator.UserName != User.Identity!.Name)
                {
                    return StatusCode(403, ForbiddenAccess);
                }

                string cadFileName = model.Name + id, cadExtension = model.CadExtension,
                imageFileName = model.Name + id, imageExtension = model.ImageExtension;
                
                await cadService.DeleteAsync(id).ConfigureAwait(false);

                env.DeleteFile("images", imageFileName, imageExtension);
                env.DeleteFile("cads", cadFileName, cadExtension);
                
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex.GetMessage());
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
