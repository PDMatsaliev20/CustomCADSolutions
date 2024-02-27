using Assimp;
using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
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
        private readonly ICategoryService categoryService;

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
            ViewBag.Categories = string.Join(" ", "All", string.Join(" ", GetCategoriesAsync())).Split();
            ViewBag.BgCategories = string.Join(" ", "Всички", string.Join(" ", bgCategories)).Split();
            return View();
        }

        public async Task<IActionResult> Category(string category)
        {
            logger.LogInformation($"Entered {category} Page");

            IEnumerable<CadModel> models = (await cadService.GetAllAsync())
                .Where(c => c.CreatorId != null &&  c.Validated)
                .OrderByDescending(c => c.CreationDate);

            Category[] categories = await GetCategoriesAsync();
            if (categories.Any(c => c.Name == category))
            {
                models = models.Where(cad => cad.Category.Name == category);
            }

            IEnumerable<CadViewModel> gallery = models
                .Select(model => new CadViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Category = model.Category.Name,
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

        private async Task<Category[]> GetCategoriesAsync()
            => (await categoryService.GetAllAsync()).ToArray();
    }
}
