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
    public class CadsController : Controller
    {
        // Services
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;

        // Addons
        private readonly ILogger<CadsController> logger;
        private readonly IMapper mapper;

        public CadsController(
            ICadService cadService,
            ICategoryService categoryService,
            ILogger<CadsController> logger)
        {
            this.cadService = cadService;
            this.categoryService = categoryService;

            MapperConfiguration config = new(cfg => cfg.AddProfile<CadAppProfile>());
            mapper = config.CreateMapper();
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] CadQueryInputModel inputQuery)
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
                Validated = true,
                Unvalidated = true,
            };
            query = await cadService.GetAllAsync(query);

            inputQuery.Categories = await categoryService.GetAllNamesAsync();
            inputQuery.TotalCount = query.TotalCount;
            inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads);

            ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
            return View(inputQuery);
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
            query = await cadService.GetAllAsync(query);

            inputQuery.TotalCount = query.TotalCount;
            inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads);
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
