using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.App.Models.Cads.View;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CustomCADSolutions.App.Areas.Designer.Controllers
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
            // Ensuring cads per page are divisible by the count of columns
            if (query.CadsPerPage % query.Cols != 0)
            {
                query.CadsPerPage = query.Cols * (query.CadsPerPage / query.Cols);
            }
            
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
            // Ensuring cads per page are divisible by the count of columns
            if (inputQuery.CadsPerPage % inputQuery.Cols != 0)
            {
                inputQuery.CadsPerPage = inputQuery.Cols * (inputQuery.CadsPerPage / inputQuery.Cols);
            }

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
