using AutoMapper;
using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.App.Mappings.CadDTOs;
using CustomCADSolutions.App.Mappings.DTOs;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CustomCADSolutions.App.APIControllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class CadsController : ControllerBase
    {
        private readonly ICadService cadService;
        private readonly IMapper mapper;

        public CadsController(ICadService cadService)
        {
            this.cadService = cadService;
            MapperConfiguration config = new(cfg => cfg.AddProfile<CadModelProfile>());
            mapper = config.CreateMapper();
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        public async Task<ActionResult<CadQueryInputModel>> GetAsync(CadQueryInputModel inputQuery, bool validated = true, bool unvalidated = false)
        {
            if (inputQuery.CadsPerPage % inputQuery.Cols != 0)
            {
                inputQuery.CadsPerPage = inputQuery.Cols * (inputQuery.CadsPerPage / inputQuery.Cols);
            }
            CadQueryModel query = await cadService.GetAllAsync(
                category: inputQuery.Category,
                creatorName: inputQuery.Creator,
                searchName: inputQuery.SearchName,
                searchCreator: inputQuery.SearchCreator,
                sorting: inputQuery.Sorting,
                validated: validated,
                unvalidated: unvalidated,
                currentPage: inputQuery.CurrentPage,
                modelsPerPage: inputQuery.CadsPerPage);

            inputQuery.TotalCadsCount = query.TotalCount;
            inputQuery.Cads = mapper.Map<CadViewModel[]>(query.CadModels);

            return inputQuery;
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status400BadRequest)]
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
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Create")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status201Created)]
        public async Task<ActionResult> PostAsync(CadImportDTO import)
        {
            CadModel cad = mapper.Map<CadModel>(import);
            cad.Bytes = import.Bytes;
            cad.CreationDate = DateTime.Now;
            cad.CreatorId = User.GetId();

            int id = await cadService.CreateAsync(cad);
            
            CadExportDTO export = mapper.Map<CadExportDTO>(cad);
            return CreatedAtAction(nameof(GetAsync), new { id }, export);
        }

        [HttpPut("Edit")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        public async Task<ActionResult> PutAsync(CadImportDTO dto)
        {
            CadModel cad = await cadService.GetByIdAsync(dto.Id);
            
            cad.Name = dto.Name;
            cad.CategoryId = dto.CategoryId;
            cad.IsValidated = false;
            cad.Coords = (dto.Coords[0], dto.Coords[1], dto.Coords[2]);
            cad.SpinAxis = dto.SpinAxis;
            await cadService.EditAsync(cad);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (await cadService.ExistsByIdAsync(id))
            {
                await cadService.DeleteAsync(id);
                return NoContent();
            }
            else return NotFound();
        }
    }
}
