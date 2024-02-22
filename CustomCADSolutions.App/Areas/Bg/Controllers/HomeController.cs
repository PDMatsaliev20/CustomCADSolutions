using Assimp;
using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CustomCADSolutions.App.Areas.Bg.Controllers
{
    [Area("Bg")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICadService cadService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly string[] bgCategories =
        {
            "Животни",
            "Герои",
            "Електроники",
            "Мода",
            "Мебели",
            "Природа",
            "Наука",
            "Спорт",
            "Играчки",
            "Автомобили",
            "Други",
        };

        public HomeController(
            ICadService cadService,
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.cadService = cadService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            logger.LogInformation("Entered Home Page");
            CadModel model = (await cadService.GetByIdAsync(269))!;
            CadViewModel view = new()
            {
                Id = model.Id,
                Name = model.Name,
                Coords = model.Coords,
                SpinAxis = model.SpinAxis,
                SpinFactor = model.SpinFactor,
            };
            return View(view);
        }

        public IActionResult Categories()
        {
            logger.LogInformation("Entered Categories Page");
            ViewBag.Categories = string.Join(" ", "All", string.Join(" ", GetCategories())).Split();
            ViewBag.BgCategories = string.Join(" ", "Всички", string.Join(" ", bgCategories)).Split();
            return View();
        }

        public async Task<IActionResult> Category(string category)
        {
            logger.LogInformation($"Entered {category} Page");
            
            List<string> categories = GetCategories().ToList();
            ViewBag.BgCategories = string.Join(" ", "Всички", string.Join(" ", bgCategories)).Split();
            ViewBag.Categories = string.Join(" ", "All", string.Join(" ", GetCategories())).Split();
            ViewBag.BgCategory = category == "All" ? 
                "Всички 3D модели" :
                bgCategories[categories.IndexOf(category)];

            IEnumerable<CadModel> models = (await cadService.GetAllAsync())
                .Where(c => c.CreatorId != null)
                .OrderByDescending(c => c.CreationDate);

            if (categories.Contains(category))
            {
                models = models.Where(cad => cad.Category.ToString() == category);
            }

            IEnumerable<CadViewModel> gallery = models
                .Select(model => new CadViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Category = model.Category.ToString(),
                    CreationDate = model.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatorName = model.Creator!.UserName,
                    Coords = model.Coords,
                    SpinAxis = model.SpinAxis,
                    SpinFactor = model.SpinFactor,
                });

            return View(gallery);
        }

        public IActionResult Privacy()
        {
            logger.LogInformation("Entered Privacy Policy Page");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            logger.LogInformation("Entered Error Page");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string[] GetCategories()
        {
            return typeof(Category).GetEnumNames();
        }
    }
}
