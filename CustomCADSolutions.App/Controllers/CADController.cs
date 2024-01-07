using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Controllers
{
    public class CADController : Controller
    {
        private readonly ICADService cadService;
        private readonly ILogger logger;

        public CADController(ICADService service, ILogger<CADModel> logger)
        {
            this.cadService = service;
            this.logger = logger;
        }

        public IActionResult Categories()
        {
            List<string> categories = new()
            {
                { "Characters" },
                { "Vehicles" },
                { "Architecture" },
                { "Nature" },
                { "Animals" },
                { "Fantasy & Sci-Fi" },
                { "Props & Assets" },
                { "Electronics & Gadgets" },
                { "Apparel & Fashion" },
                { "Toys & Games" },
                { "Abstract & Artistic" },
                { "Food & Culinary" },
                { "Sports & Recreation" },
                { "Medical & Anatomy" },
                { "Textures & Materials" },
            };
            return View("Categories", categories);
        }

        public IActionResult Category()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            CADModel model = new();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(CADModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            cadService.CreateAsync(model);
            return RedirectToAction("Sent", model);
        }

        [HttpGet]
        public IActionResult Sent(CADModel model)
        {
            return View(model);
        }

        [HttpPatch]
        public IActionResult Edit(CADModel model)
        {
            return View(model);
        }
    }
}
