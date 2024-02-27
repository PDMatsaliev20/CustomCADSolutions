using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomCADSolutions.App.Controllers
{   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(
            ICadService cadService,
            ICategoryService categoryService,
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.cadService = cadService;
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "User");
            }

            if (User.IsInRole("Designer"))
            {
                return RedirectToAction("Categories");
            }

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

        public async Task<IActionResult> Categories()
        {
            logger.LogInformation("Entered Categories Page");
            
            Category[] categories = await GetCategories();
            ViewBag.Categories = string.Join(" ", "All", string.Join(" ", categories.Select(c => c.Name)));
            
            return View();
        }

        public async Task<IActionResult> Category(string category)
        {
            logger.LogInformation($"Entered {category} Page");
            ViewBag.Category = category;

            IEnumerable<CadModel> models = (await cadService.GetAllAsync())
                .Where(c => c.Creator != null && c.Validated)
                .OrderByDescending(c => c.CreationDate);

            IEnumerable<string> categories = (await GetCategories()).Select(c => c.Name);
            if (categories.Contains(category))
            {
                models = models.Where(cad => cad.Category.ToString() == category);
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

        private async Task<Category[]> GetCategories()
            => (await categoryService.GetAllAsync()).ToArray();
    }
}
