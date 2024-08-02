using AutoMapper;
using CustomCADs.App.Mappings;
using CustomCADs.App.Models.Cads.View;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.App.Areas.Designer.Controllers
{
    [Area("Designer")]
    [Authorize(Roles = RoleConstants.Designer)]
    public class CadsController(
        ICadService cadService,
        ICategoryService categoryService,
        ILogger<CadsController> logger) : Controller
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg
                => cfg.AddProfile<CadAppProfile>())
            .CreateMapper();

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] CadQueryInputModel queryModel)
        {
            CadQuery query = new()
            {
                Creator = queryModel.Creator,
            };
            SearchModel search = new()
            {
                Category = queryModel.Category,
                Name = queryModel.SearchName,
                Owner = queryModel.SearchCreator,
                Sorting = queryModel.Sorting.ToString(),
            };
            PaginationModel pagination = new()
            {
                Page = queryModel.CurrentPage,
                Limit = queryModel.CadsPerPage,
            };

            CadResult result = await cadService.GetAllAsync(query, search, pagination);

            queryModel.Categories = await categoryService.GetAllNamesAsync();
            queryModel.TotalCount = result.Count;
            queryModel.Cads = mapper.Map<CadViewModel[]>(result.Cads);

            ViewBag.Sortings = typeof(Sorting).GetEnumNames();
            return View(query);
        }

        [HttpGet]
        public async Task<IActionResult> Submitted([FromQuery] CadQueryInputModel inputQuery)
        {
            CadQuery query = new()
            {
                Creator = inputQuery.Creator,
                Status = CadStatus.Unchecked,
            };
            SearchModel search = new()
            {
                Category = inputQuery.Category,
                Name = inputQuery.SearchName,
                Owner = inputQuery.SearchCreator,
                Sorting = inputQuery.Sorting.ToString(),
            };
            PaginationModel pagination = new()
            {
                Page = inputQuery.CurrentPage,
                Limit = inputQuery.CadsPerPage,
            };

            CadResult result = await cadService.GetAllAsync(query, search, pagination);

            inputQuery.TotalCount = result.Count;
            inputQuery.Cads = mapper.Map<CadViewModel[]>(result.Cads);
            inputQuery.Categories = await categoryService.GetAllNamesAsync();

            ViewBag.Sortings = typeof(Sorting).GetEnumNames();
            return View(inputQuery);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateCad(int cadId)
        {
            CadModel model = await cadService.GetByIdAsync(cadId);

            model.Status = CadStatus.Validated;
            await cadService.EditAsync(cadId, model);

            return RedirectToAction(nameof(Submitted));
        }
    }
}
