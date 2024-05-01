using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.App.Models.Cads.Input;
using CustomCADSolutions.App.Models.Cads.View;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Hubs;

namespace CustomCADSolutions.App.Controllers
{
    [Authorize(Roles = "Contributor,Designer")]
    public class CadsController : Controller
    {
        // Services
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;
        private readonly CadsHubHelper statisticsService;
        
        // Addons
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public CadsController(
            ICadService cadService,
            ICategoryService categoryService,
            CadsHubHelper statisticsService,
            ILogger<CadsController> logger)
        {
            this.cadService = cadService;
            this.categoryService = categoryService;
            this.statisticsService = statisticsService;

            MapperConfiguration config = new(cfg => cfg.AddProfile<CadAppProfile>());
            this.mapper = config.CreateMapper();
            this.logger = logger;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateCoords(int id, int x, int y, int z)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            model.Coords = new[] { x, y, z };

            await cadService.EditAsync(id, model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CadQueryInputModel inputQuery)
        {
            // Ensuring cads per page are divisible by the count of columns
            if (inputQuery.CadsPerPage % inputQuery.Cols != 0)
            {
                inputQuery.CadsPerPage = inputQuery.Cols * (inputQuery.CadsPerPage / inputQuery.Cols);
            }

            CadQueryModel query = new()
            {
                Category = inputQuery.Category,
                SearchName = inputQuery.SearchName,
                Sorting = inputQuery.Sorting,
                CurrentPage = inputQuery.CurrentPage,
                CadsPerPage = inputQuery.CadsPerPage,
                Creator = User.Identity!.Name,
                Validated = true,
                Unvalidated = true,
            };
            query = await cadService.GetAllAsync(query);

            inputQuery.Categories = await categoryService.GetAllNamesAsync();
            inputQuery.TotalCount = query.TotalCount;
            inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads);

            ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
            ViewBag.Category = inputQuery.Category;
            return View(inputQuery);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            return View(mapper.Map<CadViewModel>(model));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new CadAddModel()
            {
                Categories = await categoryService.GetAllAsync()
            });
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Add(CadAddModel input)
        {
            int maxFileSize = 10_000_000;
            if (input.CadFile.Length > maxFileSize)
            {
                ModelState.AddModelError(nameof(input.CadFile),
                    $"3d model cannot be over {maxFileSize / 1_000_000}MB");
            }

            if (!ModelState.IsValid)
            {
                input.Categories = await categoryService.GetAllAsync();
                return View(input);
            }

            if (input.CadFile == null || input.CadFile.Length <= 0)
            {
                return BadRequest("Invalid 3d model");
            }

            CadModel model = mapper.Map<CadModel>(input);
            model.Bytes = await input.CadFile.GetBytesAsync();
            model.CreatorId = User.GetId(); 
            model.IsValidated = User.IsInRole("Designer");
            model.CreationDate = DateTime.Now;

            await cadService.CreateAsync(model);
            await statisticsService.SendStatistics(User.GetId());

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            if (model.CreatorId != User.GetId())
            {
                return Forbid("You don't have access to this model");
            }

            CadEditModel input = mapper.Map<CadEditModel>(model);
            input.Categories = await categoryService.GetAllAsync();

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CadEditModel input)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            model.Name = input.Name;
            model.CategoryId = input.CategoryId;
            model.Price = input.Price;

            await cadService.EditAsync(id, model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await cadService.DeleteAsync(id);
            await statisticsService.SendStatistics(User.GetId());

            return RedirectToAction(nameof(Index));
        }
    }
}
