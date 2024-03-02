using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CustomCADSolutions.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICadService cadService;
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(
            ICadService cadService,
            IOrderService orderService,
            ICategoryService categoryService,
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.cadService = cadService;
            this.orderService = orderService;
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }

            if (User.IsInRole("Designer"))
            {
                return RedirectToAction("Categories");
            }

            logger.LogInformation("Entered Home Page");

            CadViewModel view = new()
            {
                Id = 0,
                Name = "Watch",
                Coords = (40, 14, 33),
                SpinAxis = 'y',
                SpinFactor = 0.01,
            };
            return View(view);
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            logger.LogInformation("Entered Categories Page");

            Category[] categories = await GetCategoriesAsync();

            ViewBag.Categories = GetCategoriesName("All", categories.Select(c => c.Name));
            ViewBag.BgCategories = GetCategoriesName("Всички", categories.Select(c => c.BgName));

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Category(string category)
        {
            logger.LogInformation($"Entered {category} Page");
            ViewBag.Category = category;

            IEnumerable<CadModel> models = (await cadService.GetAllAsync())
                .Where(c => c.Creator != null && c.Validated)
                .OrderByDescending(c => c.CreationDate);

            IEnumerable<string> categories = (await GetCategoriesAsync()).Select(c => c.Name);
            if (categories.Contains(category))
            {
                models = models.Where(cad => cad.Category.Name == category);
            }

            IEnumerable<CadViewModel> gallery = models
                .Select(model => new CadViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Category = model.Category.Name,
                    BgCategory = model.Category.BgName,
                    CreationDate = model.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatorName = model.Creator!.UserName,
                    Coords = model.Coords,
                    SpinAxis = model.SpinAxis,
                    SpinFactor = model.SpinFactor,
                });

            return View(gallery);
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return LocalRedirect(returnUrl);
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

        public async Task<Category[]> GetCategoriesAsync()
            => (await categoryService.GetAllAsync()).ToArray();

        private string GetCategoriesName(string all, IEnumerable<string> names)
            => string.Join(" ", all, string.Join(" ", names));

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
