using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CustomCADSolutions.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICadService cadService;
        private readonly UserManager<IdentityUser> userManager;

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
            //IdentityUser Oracle3000 = await userManager.FindByIdAsync("");
            //await AssignUserToRoleAsync(Oracle3000, "Contributer");
            return View();
        }

        public IActionResult Categories()
        {
            logger.LogInformation("Entered Categories Page");
            ViewBag.Categories = string.Join(" ", "All", string.Join(" ", typeof(Category).GetEnumNames())).Split();
            return View();
        }

        public async Task<IActionResult> Category(string category)
        {
            logger.LogInformation($"Entered {category} Page");
            ViewBag.Category = category;

            IEnumerable<CadModel> models = (await cadService.GetAllAsync())
                .Where(c => c.CreatorId != null);

            IEnumerable<string> categories = typeof(Category).GetEnumNames();
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

        private async Task AssignUserToRoleAsync(IdentityUser user, string role)
        {
            if (!await userManager.IsInRoleAsync(user, role))
            {
                IdentityResult result = await userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    logger.LogInformation("Add To Role Gone Wrong");
                }
            }
            else logger.LogInformation("User Already In Role");
        }
    }
}
