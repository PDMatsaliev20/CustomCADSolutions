using AutoMapper;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.App.Mappings.CadDTOs;
using CustomCADSolutions.App.Models.Cads.View;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CustomCADSolutions.App.APIControllers
{
    [ApiController]
    [Route("[controller]")]
    public class CadsAPIController : ControllerBase
    {
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public CadsAPIController(ICadService cadService, 
            ICategoryService categoryService, 
            IOrderService orderService)
        {
            this.cadService = cadService;
            this.categoryService = categoryService;
            this.orderService = orderService;
            MapperConfiguration config = new(cfg => cfg.AddProfile<CadDTOProfile>());
            mapper = config.CreateMapper();
        }

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        public async Task<ActionResult<CadQueryDTO>> GetAsync([FromQuery] CadQueryInputModel inputQuery)
        {
            inputQuery.Categories = (await categoryService.GetAllAsync()).Select(c => c.Name);
            if (inputQuery.CadsPerPage % inputQuery.Cols != 0)
            {
                inputQuery.CadsPerPage = inputQuery.Cols * (inputQuery.CadsPerPage / inputQuery.Cols);
            }

            CadQueryModel query = new()
            {
                Category = inputQuery.Category,
                Creator = inputQuery.Creator,
                LikeName = inputQuery.SearchName,
                LikeCreator = inputQuery.SearchCreator,
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

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status201Created)]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> PostAsync(CadImportDTO import)
        {
            CadModel cad = mapper.Map<CadModel>(import);

            cad.CreationDate = DateTime.Now;
            cad.Id = await cadService.CreateAsync(cad);
            cad = await cadService.GetByIdAsync(cad.Id);

            CadExportDTO export = mapper.Map<CadExportDTO>(cad);
            return CreatedAtAction(nameof(GetAsync), new { export.Id }, export);
        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> PutAsync(CadImportDTO dto)
        {
            try
            {
                CadModel cad = await cadService.GetByIdAsync(dto.Id);

                if (dto.CreatorId != cad.CreatorId)
                {
                    return Forbid();
                }

                cad.Name = dto.Name;
                cad.CategoryId = dto.CategoryId;
                cad.Coords = dto.Coords;
                cad.SpinAxis = dto.SpinAxis;
                cad.Price = dto.Price;
                cad.Color = Color.FromArgb(1, dto.RGB[0], dto.RGB[1], dto.RGB[2]);
                await cadService.EditAsync(dto.Id, cad);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //[HttpPatch("{id}")]
        //[Consumes("application/json")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(404)]
        //[IgnoreAntiforgeryToken]
        //public async Task<ActionResult> PatchAsync(int id, CadImportDTO dto)
        //{
        //    try
        //    {
        //        CadModel cad = await cadService.GetByIdAsync(dto.Id);

        //        if (dto.CreatorId != cad.CreatorId)
        //        {
        //            return Forbid();
        //        }

        //        cad.Name = dto.Name;
        //        cad.CategoryId = dto.CategoryId;
        //        cad.Coords = dto.Coords;
        //        cad.SpinAxis = dto.SpinAxis;
        //        await cadService.EditPartiallyAsync(cad);

        //        return NoContent();
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}

        [HttpDelete("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status400BadRequest)]
        [IgnoreAntiforgeryToken]
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
