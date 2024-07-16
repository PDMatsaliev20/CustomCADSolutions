using AutoMapper;
using CustomCADs.App.Mappings;
using CustomCADs.App.Models.Cads.View;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
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
        public async Task<IActionResult> All([FromQuery] CadQueryInputModel query)
        {
            CadQueryResult result = await cadService.GetAllAsync(new()
            {
                Category = query.Category,
                Creator = query.Creator,
                SearchName = query.SearchName,
                SearchCreator = query.SearchCreator,
                Sorting = query.Sorting,
                CurrentPage = query.CurrentPage,
                CadsPerPage = query.CadsPerPage,
                Validated = true,
                Unvalidated = true,
            });

            query.Categories = await categoryService.GetAllNamesAsync();
            query.TotalCount = result.Count;
            query.Cads = mapper.Map<CadViewModel[]>(result.Cads);

            ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
            return View(query);
        }

        [HttpGet]
        public async Task<IActionResult> Submitted([FromQuery] CadQueryInputModel inputQuery)
        {
            CadQueryModel query = new()
            {
                Category = inputQuery.Category,
                Creator = inputQuery.Creator,
                SearchName = inputQuery.SearchName,
                SearchCreator = inputQuery.SearchCreator,
                Sorting = inputQuery.Sorting,
                CurrentPage = inputQuery.CurrentPage,
                CadsPerPage = inputQuery.CadsPerPage,
                Validated = false,
                Unvalidated = true,
            };
            CadQueryResult result = await cadService.GetAllAsync(query);

            inputQuery.TotalCount = result.Count;
            inputQuery.Cads = mapper.Map<CadViewModel[]>(result.Cads);
            inputQuery.Categories = await categoryService.GetAllNamesAsync();

            ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
            return View(inputQuery);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateCad(int cadId)
        {
            CadModel model = await cadService.GetByIdAsync(cadId);

            model.IsValidated = true;
            await cadService.EditAsync(cadId, model);

            return RedirectToAction(nameof(Submitted));
        }
    }
}
