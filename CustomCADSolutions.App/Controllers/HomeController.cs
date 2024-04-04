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
using System.Net;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using static CustomCADSolutions.App.Constants.Paths;
using System.Text.Json;
using CustomCADSolutions.App.Mappings.CadDTOs;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using CustomCADSolutions.Infrastructure.Data.Models;
using Humanizer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace CustomCADSolutions.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public HomeController(
            ICadService cadService,
            ICategoryService categoryService,
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            HttpClient httpClient)
        {
            this.logger = logger;
            this.cadService = cadService;
            this.categoryService = categoryService;
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

            CadModel model = await cadService.GetByIdAsync(1);
            ViewBag.Chair = new CadViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Coords = model.Coords,
                SpinAxis = model.SpinAxis,
                Category = model.Category.Name,
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
            var query = await httpClient.TryGetFromJsonAsync<CadQueryDTO>(path);
            if (query != null)
            {
                inputQuery.TotalCount = query.TotalCount;
                inputQuery.Categories = (await categoryService.GetAllAsync()).Select(c => c.Name);
                inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads);

                ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
                ViewBag.Category = inputQuery.Category;
                return View(inputQuery);
            }
            else return BadRequest();
        }

        [HttpGet]
        public async Task<FileResult> DownloadCad(int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            byte[] bytes = model.Bytes!;

            return File(bytes, "application/octet-stream", $"{model.Name}.stl");
        }

        [HttpGet]
        public async Task<IActionResult> CadDetails(int id)
        {
            var dto = await httpClient.GetFromJsonAsync<CadExportDTO>($"{CadsAPIPath}/{id}");

            if (dto != null)
            {
                return View(mapper.Map<CadViewModel>(dto));
            }
            else return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeColor(int id, string colorParam, string area)  
        {
            logger.LogWarning(colorParam);
            string path = $"{CadsAPIPath}/{id}";
            var export = await httpClient.TryGetFromJsonAsync<CadExportDTO>(path);
            if (export != null)
            {
                Color color = ColorTranslator.FromHtml(colorParam);
                CadImportDTO import = new() 
                {
                    Id = export.Id,
                    CreatorId = export.CreatorId,
                    Name = export.Name,
                    Coords = export.Coords,
                    SpinAxis = export.SpinAxis,
                    RGB = new byte[] { color.R, color.B, color.G } ,
                    CategoryId = (await categoryService.GetByNameAsync(export.CategoryName)).Id,
                };

                var response = await httpClient.PutAsJsonAsync($"{CadsAPIPath}/Edit", import);
                try
                {
                    response.EnsureSuccessStatusCode();
                    return RedirectToAction("CadDetails", new { area = area, id = export.Id });
                }
                catch 
                {
                    return BadRequest(response.StatusCode);
                }
            }
            else return NotFound();
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

            IdentityUser user = await userManager.FindByIdAsync(User.GetId());
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