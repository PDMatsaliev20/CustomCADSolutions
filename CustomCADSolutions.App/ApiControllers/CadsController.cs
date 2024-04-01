using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.ApiControllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class CadsController : ControllerBase
    {
        private readonly ICadService cadService;

        public CadsController(ICadService cadService)
        {
            this.cadService = cadService;
        }

        [HttpGet("All")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CadQueryInputModel>> Get(CadQueryInputModel inputQuery, bool validated = true, bool unvalidated = false)
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
            inputQuery.Cads = query.CadModels
                .Select(m => new CadViewModel
                {
                    Id = m.Id,
                    Cad = m.Bytes!,
                    Name = m.Name,
                    Category = m.Category.Name,
                    CreationDate = m.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatorName = m.Creator!.UserName,
                    Coords = m.Coords,
                    SpinAxis = m.SpinAxis,
                    IsValidated = m.IsValidated,
                    RGB = m.Color.GetColorBytes(),
                }).ToArray();

            return inputQuery;
        }

        [HttpGet("Single")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CadViewModel>> Get(int id)
        {
            CadModel cad = await cadService.GetByIdAsync(id);
            CadViewModel view = new()
            {
                Id = cad.Id,
                Cad = cad.Bytes!,
                Name = cad.Name,
                Category = cad.Category.Name,
                CreationDate = cad.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                CreatorName = cad.Creator!.UserName,
                Coords = cad.Coords,
                SpinAxis = cad.SpinAxis,
                IsValidated = cad.IsValidated,
                RGB = cad.Color.GetColorBytes(),
            };

            return view;
        }

        [HttpPost("Create")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(201)]
        public async Task<ActionResult> Post(CadInputModel input)
        {
            byte[] bytes = await input.CadFile.GetBytesAsync();
            CadModel cad = new() 
            {
                Bytes = bytes,
                Name = input.Name,
                CategoryId = input.CategoryId,
                IsValidated = false,
                CreationDate = DateTime.Now,
                CreatorId = User.GetId()
            };
            await cadService.CreateAsync(cad);

            return CreatedAtAction("Get", new { id = cad.Id }, cad);
        }

        [HttpPut("Edit")]
        [Consumes("application/json")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Put(CadInputModel input)
        {
            CadModel cad = await cadService.GetByIdAsync(input.Id);
            
            cad.Name = input.Name;
            cad.CategoryId = input.CategoryId;
            cad.IsValidated = false;
            cad.Coords = (input.X, input.Y, input.Z);
            cad.SpinAxis = input.SpinAxis;
            await cadService.EditAsync(cad);
            
            return NoContent();
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(int id)
        {
            await cadService.DeleteAsync(id);
            return NoContent();
        }
    }
}
