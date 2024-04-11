using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.App.Mappings.CadDTOs;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using System.Drawing;
using CustomCADSolutions.Infrastructure.Data.Models;
using Stripe;
using CustomCADSolutions.App.Models.Cads.View;

namespace CustomCADSolutions.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly HttpClient httpClient;
        private readonly IConfiguration config;
        private readonly IMapper mapper;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpClient = httpClient;
            MapperConfiguration config = new(cfg => cfg.AddProfile<CadDTOProfile>());
            mapper = config.CreateMapper();
            this.config = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dto = await httpClient.GetFromJsonAsync<CadExportDTO>($"{CadsAPIPath}/{1}");
            ViewBag.Chair = new CadViewModel()
            {
                Id = dto.Id,
                Name = dto.Name,
                Coords = dto.Coords,
                SpinAxis = dto.SpinAxis,
                Category = dto.CategoryName,
                RGB = new byte[] { 143, 124, 239 },
            };

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Category([FromQuery] CadQueryInputModel inputQuery)
        {
            Dictionary<string, string> parameters = new()
            {
                ["validated"] = "true",
            };

            string path = CadsAPIPath + HttpContext.SecureQuery(parameters.ToArray());
            var query = await httpClient.GetFromJsonAsync<CadQueryDTO>(path);
            if (query != null)
            {
                var categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath);
                inputQuery.Categories = categories!.Select(c => c.Name);
                inputQuery.TotalCount = query.TotalCount;
                inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads);

                ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
                ViewBag.Category = inputQuery.Category;
                return View(inputQuery);
            }
            else return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> DownloadCad(int id)
        {
            var model = await httpClient.GetFromJsonAsync<CadExportDTO>($"{CadsAPIPath}/{id}");
            if (model != null)
            {
                byte[] bytes = model.Bytes!;

                return File(bytes, "application/octet-stream", $"{model.Name}.stl");
            }
            else return BadRequest();
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> MakeContributor()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return View();
            }

            AppUser user = await userManager.FindByIdAsync(User.GetId().ToString());
            IEnumerable<string> roles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRoleAsync(user, roles.Single());
            await userManager.AddToRoleAsync(user, "Contributor");

            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Cads", new { area = "Contributor" });
        }

        public async Task<IActionResult> AddUsers(string returnUrl)
        {
            string role, email, password;

            email = "ivanangelov414@gmail.com";
            password = config["passwords:admin"];
            role = "Administrator";
            await userManager.AddUserAsync("NinjataBG", email, password, role);

            email = "ivanangelov413@gmail.com"; 
            password = config["passwords:designer"];
            role = "Designer";
            await userManager.AddUserAsync(role, email, password, role);

            email = "ivanangelov412@gmail.com";
            password = config["passwords:contributor"];
            role = "Contributor";
            await userManager.AddUserAsync(role, email, password, role);

            email = "ivanangelov411@gmail.com";
            password = config["passwords:Client"];
            role = "Client";
            await userManager.AddUserAsync(role, email, password, role);

            return LocalRedirect(returnUrl);
        }

        private async Task AddAdminAsync()
        {
            string username = "NinjataBG", email = "ivanangelov414@gmail.com",
            password = config["passwords:admin"], role = "Administrator";

            var admin = await userManager.FindByNameAsync(username);
            if (admin == null)
            {
                admin = new AppUser
                {
                    UserName = username,
                    Email = email,
                };

                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, role);
                }
            }
        }

        private async Task AddContributorAsync()
        {
            string username = "NinjataBG", email = "ivanangelov414@gmail.com",
            password = config["admin_password"], role = "Administrator";

            var admin = await userManager.FindByNameAsync(username);
            if (admin == null)
            {
                admin = new AppUser
                {
                    UserName = username,
                    Email = email,
                };

                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, role);
                }
            }
        }

        private async Task AddClientAsync()
        {
            string username = "NinjataBG", email = "ivanangelov414@gmail.com",
            password = config["admin_password"], role = "Administrator";

            var admin = await userManager.FindByNameAsync(username);
            if (admin == null)
            {
                admin = new AppUser
                {
                    UserName = username,
                    Email = email,
                };

                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, role);
                }
            }
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