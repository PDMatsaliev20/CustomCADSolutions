using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.API.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CategoryModel[]>> GetAsync()
        {
            IEnumerable<CategoryModel> categories = await categoryService.GetAllAsync();
            return categories.ToArray();
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CategoryModel>> GetAsync(int id)
        {
            try
            {
                CategoryModel category = await categoryService.GetByIdAsync(id);
                return category;
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
