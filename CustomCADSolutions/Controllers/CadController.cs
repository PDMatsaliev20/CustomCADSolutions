using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using CustomCADSolutions.App.Models;
using CustomCADSolutions.Models;

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

        public async Task<IActionResult> Index(string category)
        {
            logger.LogInformation("Entered Explore Page");
            await service.UpdateCads();

            IEnumerable<CadModel> cads = (await service.GetAllAsync()).Where(cad => cad.CreationDate.HasValue);
            if (category != "All")
            {
                cads = cads.Where(cad => category == cad.Category.ToString());
            }

            IEnumerable<CadViewModel> views = cads
                .Select(cad => new CadViewModel
                {
                    Name = cad.Name,
                    Url = cad.Url,
                    Category = cad.Category.ToString(),
                    CreatedDate = (cad.CreationDate ?? throw new NullReferenceException()).ToString("dd:MM:yyyy HH:mm:ss"),
                })
                .OrderBy(cad => cad.CreatedDate);

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
                return Error();
            }

            CadModel cad = new()
            {
                Name = input.Name,
                Category = input.Category!.Value,
                CreationDate = DateTime.Now,
                Url = input.Url,
            };
            await service.CreateAsync(cad);

            logger.LogInformation("Submitted 3d Model");
            return RedirectToAction(nameof(Index), routeValues: cad.Category);
        }

        private IActionResult Error() => RedirectToAction("Error", "Home");
    }
}
