using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Others;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;

    [ApiController]
    [Route("API/[controller]")]
    public class HomeController(ICadService cadService, ICategoryService categoryService, IMapper mapper) : ControllerBase
    {
        [HttpGet("Cad")]
        [ProducesResponseType(Status200OK)]
        public ActionResult<CadGetDTO> GetHomeCadAsync()
            => new CadGetDTO()
            {
                CadPath = "/files/HomeCAD.glb",
                Coords = [2, 16, 33],
                PanCoords = [0, 6, -3]
            };

        [HttpGet("Gallery")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadQueryResultDTO>> GetGalleryAsync([FromQuery] PaginationModel pagination, string? sorting, string? category, string? name, string? creator)
        {
            CadQuery query = new() { Status = CadStatus.Validated };
            SearchModel search = new() { Category = category, Name = name, Owner = creator, Sorting = sorting ?? "" };
            
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
                return StatusCode(500, ex.GetMessage());
            }
        }

        [HttpGet("Gallery/{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadGetDTO>> GetGalleryCadAsync(int id)
        {
            try
            {
                CadModel model = await cadService.GetByIdAsync(id).ConfigureAwait(false);
                return mapper.Map<CadGetDTO>(model);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
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

        [HttpGet("Categories")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CategoryDTO[]>> GetCategoriesAsync()
        {
            try
            {
                IEnumerable<CategoryModel> categories = await categoryService.GetAllAsync().ConfigureAwait(false);
                return mapper.Map<CategoryDTO[]>(categories);
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

        [HttpGet("Sortings")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public ActionResult<string[]> GetSortingsAsync()
            => Enum.GetNames<Sorting>();
    }
}
