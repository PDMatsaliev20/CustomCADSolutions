using AutoMapper;
using CustomCADSolutions.Core.Mappings;
using CustomCADSolutions.Core.Mappings.CadDTOs;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Serialization;

namespace CustomCADSolutions.App.APIControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CadsController : ControllerBase
    {
        private readonly ICadService cadService;
        private readonly IMapper mapper;

        public CadsController(ICadService cadService)
        {
            this.cadService = cadService;
            MapperConfiguration config = new(cfg => cfg.AddProfile<CadCoreProfile>());
            mapper = config.CreateMapper();
        }

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        public async Task<ActionResult<CadQueryDTO>> GetAsync([FromQuery] CadQueryModel inputQuery)
        {
            if (inputQuery.CadsPerPage % 3 != 0)
            {
                inputQuery.CadsPerPage = 3 * (inputQuery.CadsPerPage / 3);
            }

            CadQueryModel query = new()
            {
                Category = inputQuery.Category,
                Creator = inputQuery.Creator,
                LikeName = inputQuery.LikeName,
                LikeCreator = inputQuery.LikeCreator,
                Sorting = inputQuery.Sorting,
                CurrentPage = inputQuery.CurrentPage,
                CadsPerPage = inputQuery.CadsPerPage,
                Validated = inputQuery.Validated,
                Unvalidated = inputQuery.Unvalidated,
            };

            query = await cadService.GetAllAsync(query);
            return mapper.Map<CadQueryDTO>(query);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<CadExportDTO>> GetAsync(int id)
        {
            try
            {
                CadModel model = await cadService.GetByIdAsync(id);
                return mapper.Map<CadExportDTO>(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult> PostAsync(CadImportDTO import)
        {
            CadModel model = mapper.Map<CadModel>(import);
            model.CreationDate = DateTime.Now;

            try
            {
                int id = await cadService.CreateAsync(model);

                model = await cadService.GetByIdAsync(id);
                CadExportDTO export = mapper.Map<CadExportDTO>(model);

                return CreatedAtAction(null, new { id }, export);
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
        public async Task<ActionResult> PutAsync(int id, CadImportDTO dto)
        {
            try
            {
                CadModel cad = await cadService.GetByIdAsync(id);
                
                cad.Name = dto.Name;
                cad.CategoryId = dto.CategoryId;
                cad.Coords = dto.Coords;
                cad.SpinAxis = dto.SpinAxis;
                cad.Price = dto.Price;
                cad.Color = Color.FromArgb(1, dto.RGB[0], dto.RGB[1], dto.RGB[2]);
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
