using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Models;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CustomCADSolutions.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICadService cadService;
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController(
            ICadService cadService,
            IOrderService orderService,
            ICategoryService categoryService,
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.logger = logger;
            this.cadService = cadService;
            this.orderService = orderService;
            this.categoryService = categoryService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrator"))
            {
                return Redirect("/Admin");
            }

            if (User.IsInRole("Designer"))
            {
                return Redirect("/Home/Categories");
            }

            logger.LogInformation("Entered Home Page");

            ViewBag.Chair = new CadViewModel()
            {
                Id = 1,
                Name = "Chair",
                Coords = (750, 300, 0),
                SpinAxis = 'y',
                SpinFactor = -0.01,
                TexturePath = "/textures/texture3.jpg",
            };

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            return View(new CadQueryInputModel() 
            {
                Categories = (await categoryService
                    .GetAllAsync())
                    .Select(c => c.Name) 
            });
        }

        [HttpGet]
        public async Task<IActionResult> Category([FromQuery] CadQueryInputModel inputQuery, string category)
        {
            inputQuery.Cols = 3;
            inputQuery.CadsPerPage = inputQuery.CadsPerPage == 4 ? 3 : inputQuery.CadsPerPage;
            inputQuery = await cadService.QueryCads(inputQuery, true, false);
            inputQuery.Categories = (await categoryService.GetAllAsync()).Select(c => c.Name);
            ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
            
            ViewBag.Category = category;
            return View(inputQuery);
        }

        [HttpGet]
        public async Task<FileResult> DownloadCad(int id)
        {
            CadModel model = (await cadService.GetByIdAsync(id));
            byte[] bytes = model.Bytes ?? throw new NullReferenceException("3d model hasn't been created yet.");

            return File(bytes, "application/octet-stream", $"{model.Name}.stl");
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> MakeContributer()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return View();
            }

            IdentityUser user = await userManager.FindByIdAsync(User.GetId());
            IEnumerable<string> roles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRoleAsync(user, roles.Single());
            await userManager.AddToRoleAsync(user, "Contributer");

            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Cad", new { area = "Contributer" });
        }

        public IActionResult Privacy()
        {
            logger.LogInformation("Entered Privacy Policy Page");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        public IActionResult StatusCodeHandler(int statusCode)
        {
            if (statusCode == 0)
            {
                ViewBag.OriginalStatusCode = "An exception was thrown";
                ViewBag.ErrorMessage = "I wonder what it could be...";
            }
            else
            {
                ViewBag.OriginalStatusCode = $"A {statusCode} error occured";
                ViewBag.ErrorMessage = statusCode switch
                {
                    400 => "Your request could not be understood by the server due to malformed syntax or other client-side errors.",
                    401 => "You do not have access to the resource you requested.",
                    404 => "The resource you requested could not be found.",
                    _ => "An error occurred.",
                } + "Sowwy";
            }
            return View();
        }

        public new IActionResult Unauthorized() => base.Unauthorized();
    }
}