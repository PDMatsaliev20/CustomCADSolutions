using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.App.Controllers
{
    public class CADController : Controller
    {
        private readonly ICADService service;
        private readonly ILogger logger;

        public CADController(ICADService service, ILogger<CADModel> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        public IActionResult Categories()
        {
            IEnumerable<string> shit = service.GetCategories().Select(c => c.Name);
            return View("Categories", shit);
        }

        public IActionResult Category(string category)
        {
            Category model = service.GetCategories().First(c => c.Name == category);
            CategoryViewModel view = new()
            {
                Name = model.Name,
                CADs = model.CADs
                    .Select(c => new CADViewModel 
                    {
                        Category = c.Category.Name,
                        Name = c.Name,
                        Url = c.Url,
                        CreatedOn = c.CreationDate.ToString("dd/MM/yyyy hh:MM:ss"),
                    })
                    .ToArray(),
            };
            return View("Category", view);
        }

        [HttpGet]
        public IActionResult Add()
        {
            CADInputModel model = new();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(CADInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            CADModel cad = new()
            {
                Name = input.Name,
                CreationDate = DateTime.Now,
                Category = service.GetCategories().First(category => category.Name == input.Category),
                Url = input.Url
            };
            service.CreateAsync(cad);

            CADViewModel view = new()
            {
                Name = cad.Name,
                Category = cad.Category.Name,
                CreatedOn = cad.CreationDate.ToString("dd/mm/yyyy hh:MM:ss")
            };

            return RedirectToAction("Sent", view);
        }

        [HttpGet]
        public IActionResult Sent(CADViewModel model)
        {
            return View(model);
        }

        [HttpPatch]
        public IActionResult Edit(CADInputModel model)
        {
            return View(model);
        }
    }
}
