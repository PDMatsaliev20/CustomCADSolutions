using AutoMapper;
using CustomCADSolutions.App.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models.Cads.View;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CadsController : Controller
    {
        // Services
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;
        
        // Addons
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public CadsController(
            ICadService cadService,
            ICategoryService categoryService,
            ILogger<CadsController> logger
            )
        {
            this.logger = logger;
            this.cadService = cadService;
            this.categoryService = categoryService;
            MapperConfiguration config = new(cfg => cfg.AddProfile<CadAppProfile>());
            this.mapper = config.CreateMapper();
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CadQueryInputModel inputQuery)
        {
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
                Validated = true,
                Unvalidated = true,
            };
            query = await cadService.GetAllAsync(query);

            return View(mapper.Map<CadViewModel[]>(query.Cads));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await cadService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
