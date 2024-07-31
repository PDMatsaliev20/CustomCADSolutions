using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Others;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;

    [ApiController]
    [Route("/API/[controller]")]
    public class CommonController(ICategoryService categoryService) : ControllerBase
    {
        private readonly IMapper mapper = new MapperConfiguration(opt
                => opt.AddProfile<OtherApiProfile>())
            .CreateMapper();

        [HttpGet("Categories")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CategoryDTO[]>> GetCategoriesAsync()
        {
            try
            {
                IEnumerable<CategoryModel> categories = await categoryService.GetAllAsync();
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

        [HttpGet("CadSortings")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public ActionResult<string[]> GetCadSortingsAsync() 
            => Enum.GetNames<CadSorting>();
    }
}
