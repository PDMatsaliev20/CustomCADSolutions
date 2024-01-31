using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using CustomCADSolutions.App.Models;
using Microsoft.AspNetCore.Authorization;

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

        public IActionResult Index()
        {
            string[] categories = typeof(Category).GetEnumNames();
            return View(categories);
        }

        public async Task<IActionResult> Category(string category)
        {
            await service.UpdateCads();

            IEnumerable<CadModel> cads = category == "All" ?
                await service.GetAllAsync() :
                (await service.GetAllAsync())
                    .Where(cad => category == cad.Category.ToString());

            ViewBag.Category = category;

            IEnumerable<CadViewModel> views = cads
                .Select(cad => new CadViewModel
                {
                    Name = cad.Name,
                    Url = cad.Url,
                    Category = cad.Category.ToString(),
                    CreatedOn =
                        (cad.CreationDate ?? throw new NullReferenceException())
                        .ToString("dd:MM:yyyy HH:mm:ss"),
                });

            return View(views);
        }

        public IActionResult Submit()
        {
            return View();
        }
    }
}
