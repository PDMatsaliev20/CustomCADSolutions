﻿using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.APIControllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesAPIController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesAPIController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<Category[]>> GetAsync()
        {
            IEnumerable<Category> categories = await categoryService.GetAllAsync();
            return categories.ToArray();
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Category>> GetAsync(int id)
        {
            return await categoryService.GetByIdAsync(id);
        }
    }
}
