using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Models;
using CustomCADSolutions.App.Models.Cads;
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

namespace CustomCADSolutions.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            HttpClient httpClient)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpClient = httpClient;
            MapperConfiguration config = new(cfg => cfg.AddProfile<CadDTOProfile>());
            mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrator"))
            {
                return Redirect("/Admin");
            }

            logger.LogInformation("Entered Home Page");

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

        [HttpGet]
        public async Task<IActionResult> CadDetails(int id)
        {
            try
            {
                var dto = await httpClient.GetFromJsonAsync<CadExportDTO>($"{CadsAPIPath}/{id}");
                return View(mapper.Map<CadViewModel>(dto));

            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeColor(int id, string colorParam, string area)
        {
            string _;
            try
            {
                _ = $"{CadsAPIPath}/{id}";
                var export = (await httpClient.GetFromJsonAsync<CadExportDTO>(_))!;

                _ = $"{CategoriesAPIPath}/{export.CategoryId}";
                var category = (await httpClient.GetFromJsonAsync<Category>(_))!;

                Color color = ColorTranslator.FromHtml(colorParam);
                CadImportDTO import = new()
                {
                    Id = export.Id,
                    CreatorId = export.CreatorId,
                    Name = export.Name,
                    Coords = export.Coords,
                    SpinAxis = export.SpinAxis,
                    RGB = new byte[] { color.R, color.G, color.B },
                    CategoryId = category.Id,
                };

                var response = await httpClient.PutAsJsonAsync(CadsAPIPath, import);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("CadDetails", new { area, id = export.Id });
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
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