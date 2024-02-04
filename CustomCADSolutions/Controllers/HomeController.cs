using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using CustomCADSolutions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomCADSolutions.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            logger.LogInformation("Entered Home Page");
            return View();
        }

        public IActionResult Privacy()
        {
            logger.LogInformation("Entered Privacy Policy Page");
            return View();
        }

        public IActionResult Categories()
        {
            logger.LogInformation("Entered Categories Page");

            string categoriesString = string.Join(" ", typeof(Category).GetEnumNames());
            string allCategoriesString = string.Join(" ", "All", categoriesString);
            
            string[] categories = allCategoriesString.Split();
            return View(categories);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            logger.LogInformation("Entered Error Page");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
