using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;
    using static ApiMessages;

    /// <summary>
    ///     Controller for CRUD operations on Cad
    /// </summary>
    /// <param name="cadService"></param>
    /// <param name="env"></param>
    /// <param name="mapper"></param>
    [Authorize(Roles = $"{Contributor},{Designer}")]
    [ApiController]
    [Route("API/[controller]")]
    public class CadsController(ICadService cadService, IWebHostEnvironment env, IMapper mapper) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(GetCadAsync).Replace("Async", "");
        
        /// <summary>
        ///     Queries the User's Cads with the specified parameters.
        /// </summary>
        /// <param name="sorting"></param>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadQueryResultDTO>> GetCadsAsync(string? sorting, string? category, string? name, int page = 1, int limit = 20)
        {
            CadQuery query = new() { Creator = User.Identity!.Name };
            SearchModel search = new() { Category = category, Name = name, Sorting = sorting ?? "" };
            PaginationModel pagination = new() { Page = page, Limit = limit };

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
        
        /// <summary>
        ///     Gets the User's most recent Cads.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Recent")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadQueryResultDTO>> GetRecentCadsAsync(int limit = 4)
        {
            CadQuery query = new() { Creator = User.Identity!.Name };
            SearchModel search = new() { Sorting = nameof(Sorting.Newest) };
            PaginationModel pagination = new() { Limit = limit };

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
        
        /// <summary>
        ///     Gets counts of the User's Cads grouped by their status.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Counts")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadCountsDTO>> GetCadsCountsAsync()
        {
            try
            {
                bool predicate(CadModel cad, CadStatus s)
                    => cad.Status == s && cad.Creator.UserName == User.Identity!.Name;

                int uncheckedCadsCounts = await cadService.Count(c => predicate(c, CadStatus.Unchecked)).ConfigureAwait(false);
                int validatedCadsCounts = await cadService.Count(c => predicate(c, CadStatus.Validated)).ConfigureAwait(false);
                int reportedCadsCounts = await cadService.Count(c => predicate(c, CadStatus.Reported)).ConfigureAwait(false);
                int bannedCadsCounts = await cadService.Count(c => predicate(c, CadStatus.Banned)).ConfigureAwait(false);

                CadCountsDTO counts = new(uncheckedCadsCounts, validatedCadsCounts, reportedCadsCounts, bannedCadsCounts);
                return counts;
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        /// <summary>
        ///     Gets a Cad by the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Creates a Cad entity in the database, max file size is 300MB.
        /// </summary>
        /// <param name="import"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Updates Name, Description, Price CategoryId and optionally Image properties of Cad.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
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
                    env.DeleteFile("images", cad.Name + id, cad.Paths.ImageExtension);
                    string imagePath = await env.UploadImageAsync(dto.Image, dto.Name + id + dto.Image.GetFileExtension()).ConfigureAwait(false);
                    await cadService.SetPathsAsync(id, cad.Paths.FilePath, imagePath).ConfigureAwait(false);
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

        /// <summary>
        ///     Updates CamCoordinates or PanCoordinates property of Cad.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">Must be either Camera or Pan.</param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status409Conflict)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PatchCadAsync(int id, string type, CoordinatesDTO coordinates)
        {
            try
            {
                CadModel model = await cadService.GetByIdAsync(id).ConfigureAwait(false);

                if (model.Creator.UserName != User.Identity!.Name)
                {
                    return StatusCode(403, ForbiddenAccess);
                }

                double x = coordinates.X, y = coordinates.Y, z = coordinates.Z;
                switch (type.Trim().ToLower())
                {
                    case "camera": model.CamCoordinates = new(x, y, z); break;
                    case "pan": model.PanCoordinates = new(x, y, z); break;
                    default: throw new ArgumentException("Parameter 'type' must be either 'Camera' or 'Pan'.");
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.GetMessage());
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

        /// <summary>
        ///     Deletes the Cad with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

                string cadFileName = model.Name + id, cadExtension = model.Paths.FileExtension,
                imageFileName = model.Name + id, imageExtension = model.Paths.ImageExtension;
                
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
