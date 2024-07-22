using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using static CustomCADs.Domain.DataConstants.RoleConstants;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CustomCADs.API.Controllers
{
    [Authorize(Roles = $"{Contributor},{Designer}")]
    [ApiController]
    [Route("API/[controller]")]
    public class CadsController(ICadService cadService, IWebHostEnvironment env) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(GetSingleAsync).Replace("Async", "");
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OtherApiProfile>();
            cfg.AddProfile<CadApiProfile>();
        }).CreateMapper();

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        public async Task<ActionResult<CadQueryResultDTO>> GetAsync([FromQuery] CadQueryDTO dto)
        {
            CadQueryModel query = mapper.Map<CadQueryModel>(dto);
            CadQueryResult result = await cadService.GetAllAsync(query);
            return mapper.Map<CadQueryResultDTO>(result);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<CadGetDTO>> GetSingleAsync(int id)
        {
            try
            {
                CadModel model = await cadService.GetByIdAsync(id);
                return mapper.Map<CadGetDTO>(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult> PostAsync([FromForm] CadPostDTO import)
        {
            CadModel model = mapper.Map<CadModel>(import);
            model.CreationDate = DateTime.Now;
            model.CreatorId = User.GetId();
            model.Extension = import.File.GetFileExtension();
            
            try
            {
                int id = await cadService.CreateAsync(model);
                model = await cadService.GetByIdAsync(id);
                
                string path = await env.UploadCadAsync(import.File, model.Name + id + model.Extension);
                await cadService.SetPathAsync(id, path);

                CadGetDTO export = mapper.Map<CadGetDTO>(model);
                return CreatedAtAction(createdAtReturnAction, new { id }, export);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> PutAsync(int id, CadPutDTO dto)
        {
            try
            {
                CadModel cad = await cadService.GetByIdAsync(id);

                if (User.Identity!.Name != cad.Creator.UserName)
                {
                    return Forbid();
                }
                
                string path = env.RenameFile(cad.Name + id + cad.Extension, dto.Name + id + cad.Extension);
                await cadService.SetPathAsync(id, path);

                cad.Name = dto.Name;
                cad.Description = dto.Description;
                cad.CategoryId = dto.CategoryId;
                cad.Price = dto.Price;
                await cadService.EditAsync(id, cad);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> PatchAsync(int id, [FromBody] List<Operation<CadModel>> operations)
        {
            JsonPatchDocument<CadModel> patchCad = new(operations, new DefaultContractResolver());
            try
            {
                CadModel model = await cadService.GetByIdAsync(id);
                patchCad.ApplyTo(model);

                IList<string> errors = cadService.ValidateEntity(model);
                if (errors.Any())
                {
                    return BadRequest(string.Join("; ", errors));
                }

                await cadService.EditAsync(id, model);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                if (await cadService.ExistsByIdAsync(id))
                {
                    CadModel model = await cadService.GetByIdAsync(id);
                    env.DeleteFile(model.Path);
                    
                    await cadService.DeleteAsync(id);
                    
                    return NoContent();
                }
                else return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
