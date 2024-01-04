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
            return View();
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
