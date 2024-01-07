using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Controllers
{
    public class CADController : Controller
    {
        private readonly ICADService cadService;
        private readonly ILogger logger;
        private readonly static Dictionary<string, HashSet<(string, string)>> categories = new()
        {
            { "Characters",            new() { ("Shit1", "fc33313a90ef4d49aa69ed89d30e6ae0") } },
            { "Vehicles",              new() { ("Shit1", "539b2257cb504e53969a8c38c1a686cc") } },
            { "Architecture",          new() { ("Shit1", "204d354e8456472a882005d15728d2df") } },
            { "Nature",                new() { ("Shit1", "5f8b72c98ec5415fbe8f1a4b9c91b501") } },
            { "Animals",               new() { ("Shit1", "132e3bb7e52f47c89bba65bbb5978e4b") } },
            { "Fantasy & Sci-Fi",      new() { ("Shit1", "9e068293efb04e4780330ddde3cda2db") } },
            { "Props & Assets",        new() { ("Shit1", "5cb4706191e4467aaa8635c3ac4464b4") } },
            { "Electronics & Gadgets", new() { ("Shit1", "49138c28de334c84955719ffcb4e7aab") } },
            { "Apparel & Fashion",     new() { ("Shit1", "800bcff1a01b4e1e97087f681cd0a4d9") } },
            { "Toys & Games",          new() { ("Shit1", "318c21574aee4f8cbcc2de05ab171cce") } },
            { "Abstract & Artistic",   new() { ("Shit1", "8f25337b683d415e8f393eff60bc2aa1") } },
            { "Food & Culinary",       new() { ("Shit1", "1b1f4185a9414ef8811eec8b7e09a6b6") } },
            { "Sports & Recreation",   new() { ("Shit1", "880bcabd62544bf3ab5246af0721ddc8") } },
            { "Medical & Anatomy",     new() { ("Shit1", "9b0b079953b840bc9a13f524b60041e4") } },
            { "Textures & Materials",  new() { ("Shit1", "504a59ef14a148809cec657d7c99367c") } },
        };

        public CADController(ICADService service, ILogger<CADModel> logger)
        {
            this.cadService = service;
            this.logger = logger;
        }

        public IActionResult Categories()
        {
            return View("Categories", categories.Keys);
        }

        public IActionResult Category(string category)
        {
            KeyValuePair<string, HashSet<(string, string)>>? categoryInfo = categories.FirstOrDefault(c => c.Key == category);

            if (categoryInfo.Equals(default))
            {
                return RedirectToAction("Error", "Home");
            }

            return View("Category", categoryInfo);
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
