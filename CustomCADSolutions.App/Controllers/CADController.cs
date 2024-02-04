using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.App.Controllers
{
    public class CadController : Controller
    {
        private readonly ICadService service;
        private readonly ILogger logger;

        public CadController(ICadService service, ILogger<CadModel> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Categories()
        {
            logger.LogInformation("Entered Categories Page");
            ViewBag.Categories = string.Join(" ", "All", string.Join(" ", typeof(Category).GetEnumNames())).Split();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string category)
        {
            logger.LogInformation("Entered Explore Page");
            ViewBag.Category = category;

            IEnumerable<CadViewModel> views = ConvertModelToView(await GetCads(category));
            return View(views);
        }

        [HttpGet]
        public IActionResult Add()
        {
            logger.LogInformation("Entered Submit Page");
            return View(new CadInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(CadInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid 3d Model");
                return View(input);
            }

            CadModel cad = new()
            {
                Name = input.Name,
                Category = input.Category,
                CreationDate = DateTime.Now,
                Url = input.Url,
            };
            await service.CreateAsync(cad);

            logger.LogInformation("Submitted 3d Model");
            return RedirectToAction(nameof(Index), routeValues: cad.Category);
        }

        private async Task<IEnumerable<CadModel>> GetCads(string category)
        {
            var cads = (await service.GetAllAsync());

            if (category != "All")
            {
                cads = cads.Where(cad => category == cad.Category.ToString());
            }

            return cads;
        }

        private static IEnumerable<CadModel> ConvertInputToModel(IEnumerable<CadInputModel> inputs)
        {
            var models = inputs
                .Select(input => new CadModel
                {
                    Name = input.Name,
                    Url = input.Url,
                    Category = input.Category,
                    CreationDate = DateTime.Now,
                })
                .OrderBy(cad => cad.CreationDate);

            return models;
        }

        private static IEnumerable<CadViewModel> ConvertModelToView(IEnumerable<CadModel> cads)
        {
            IEnumerable<CadViewModel> views = cads
                .Select(cad => new CadViewModel
                {
                    Name = cad.Name,
                    Category = cad.Category.ToString(),
                    Url = cad.Url!,
                });

            return views;
        }
    }
}
