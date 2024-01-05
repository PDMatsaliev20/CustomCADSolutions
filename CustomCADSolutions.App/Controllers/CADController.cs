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

        public IActionResult Index()
        {
            List<(string, string)> categories = new()
            {
                { ("Characters", "Categories/Characters.png") },
                { ("Vehicles", "Categories/Vehicles.png") },
                { ("Architecture", "Categories/Architecture.png") },
                { ("Nature", "Categories/Nature.png") },
                { ("Animals", "Categories/Animals.png") },
                { ("Fantasy & Sci-Fi", "Categories/Fantasy & Sci-Fi.png") },
                { ("Props & Assets", "Categories/Props & Assets.png") },
                { ("Machinery & Industrial", "Categories/Machinery & Industrial.png") },
                { ("Electronics & Gadgets", "Categories/Electronics & Gadgets.png") },
                { ("Apparel & Fashion", "Categories/Apparel & Fashion.png") },
                { ("Toys & Games", "Categories/Toys & Games.png") },
                { ("Abstract & Artistic", "Categories/Abstract & Artistic.png") },
                { ("Food & Culinary", "Categories/Food & Culinary.png") },
                { ("Sports & Recreation", "Categories/Sports & Recreation.png") },
                { ("Medical & Anatomy", "Categories/Medical & Anatomy.png") },
                { ("Textures & Materials", "Categories/Textures & Materials.png") }
            };
            return View(categories);
        }

        public IActionResult Categories()
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
