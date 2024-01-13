using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using CustomCADSolutions.App.Models;

namespace CustomCADSolutions.App.Controllers
{
    public class CADController : Controller
    {
        private readonly ICADService service;
        private readonly ILogger logger;

        public CADController(ICADService service, ILogger<CadModel> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        public IActionResult Categories()
            {
            string[] categories = typeof(Category).GetEnumNames();
            return View(categories);
        }

        public async Task<IActionResult> Index(string category)
        {
            IEnumerable<CADViewModel> models = 
                (await service.GetAllAsync())
                .Where(cad => cad.Category.ToString() == category)
                .Select(cad => new CADViewModel
                {
                    Name = cad.Name,
                    Url = cad.Url,
                    Category = cad.Category.ToString(),
                    CreatedOn = (cad.CreationDate ?? throw new NullReferenceException()).ToString("dd:MM:yyyy HH:mm:ss"),
                });

            return View(models);
        }
    }
}
