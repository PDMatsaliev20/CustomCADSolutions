using AutoMapper;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Others;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.API.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        private readonly IMapper mapper = new MapperConfiguration(opt => 
                opt.AddProfile<OtherApiProfile>())
            .CreateMapper();

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CategoryDTO[]>> GetAsync()
        {
            IEnumerable<CategoryModel> categories = await categoryService.GetAllAsync();
            return mapper.Map<CategoryDTO[]>(categories);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CategoryDTO>> GetAsync(int id)
        {
            try
            {
                CategoryModel category = await categoryService.GetByIdAsync(id);
                return mapper.Map<CategoryDTO>(category);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpGet("ByName/{name}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CategoryDTO>> GetAsync(string name)
        {
            try
            {
                CategoryModel category = await categoryService.GetByNameAsync(name);
                return mapper.Map<CategoryDTO>(category);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
