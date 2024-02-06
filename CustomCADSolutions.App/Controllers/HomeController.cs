using CustomCADSolutions.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomCADSolutions.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private const string title = "Where Your Imagination Comes to Life!";
        private const string message = "In a world sculpted by pixels and imagination, each 3D model is a bridge between dream and reality, transforming visions into virtual 3D masterpieces!";

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            logger.LogInformation("Entered Home Page");
            ViewBag.Title = title;
            ViewBag.Message = message;
            return View();
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
    }
}
