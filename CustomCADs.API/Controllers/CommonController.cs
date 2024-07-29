﻿using AutoMapper;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Others;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.API.Controllers
{
    [ApiController]
    [Route("/API/[controller]")]
    public class CommonController(ICategoryService categoryService) : ControllerBase
    {
        private readonly IMapper mapper = new MapperConfiguration(opt
                => opt.AddProfile<OtherApiProfile>())
            .CreateMapper();

        [HttpGet("Categories")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CategoryDTO[]>> GetCategoriesAsync()
        {
            IEnumerable<CategoryModel> categories = await categoryService.GetAllAsync();
            return mapper.Map<CategoryDTO[]>(categories);
        }

        [HttpGet("CadSortings")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public ActionResult<string[]> GetCadSortingsAsync() 
            => Enum.GetNames<CadSorting>();
    }
}
