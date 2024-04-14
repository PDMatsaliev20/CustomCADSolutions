﻿using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.App.Models.Cads.View;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Controllers
{
    public class HomeController : Controller
    {
        // Services
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;

        // Managers
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        // Additional
        private readonly IMapper mapper;
        private readonly ILogger<HomeController> logger;

        public HomeController(
            ICadService cadService,
            ICategoryService categoryService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<HomeController> logger)
        {
            this.cadService = cadService;
            this.categoryService = categoryService;

            this.userManager = userManager;
            this.signInManager = signInManager;

            MapperConfiguration config = new(cfg => cfg.AddProfile<CadDTOProfile>());
            mapper = config.CreateMapper();
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new CadViewModel()
            {
                Id = 1,
                Name = "Chair",
                SpinAxis = 'y',
                RGB = new byte[] { 143, 124, 239 },
                Coords = new[] { 750, 300, 0 }
            });
        }

        [HttpGet]
        public async Task<IActionResult> Category([FromQuery] CadQueryInputModel inputQuery)
        {
            // Action-specific parameters
            inputQuery.Validated = true;

            // Ensuring cads per page are divisible by the count of columns
            if (inputQuery.CadsPerPage % inputQuery.Cols != 0)
            {
                inputQuery.CadsPerPage = inputQuery.Cols * (inputQuery.CadsPerPage / inputQuery.Cols);
            }

            CadQueryModel query = new()
            {
                Category = inputQuery.Category,
                Creator = inputQuery.Creator,
                LikeName = inputQuery.SearchName,
                LikeCreator = inputQuery.SearchCreator,
                Sorting = inputQuery.Sorting,
                CurrentPage = inputQuery.CurrentPage,
                CadsPerPage = inputQuery.CadsPerPage,
                Validated = inputQuery.Validated,
                Unvalidated = inputQuery.Unvalidated,
            };
            query = await cadService.GetAllAsync(query);

            inputQuery.Categories = await categoryService.GetAllNamesAsync();
            inputQuery.TotalCount = query.TotalCount;
            inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads);

            ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
            ViewBag.Category = inputQuery.Category;
            return View(inputQuery);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadCad(int id)
        {
            try
            {
                CadModel model = await cadService.GetByIdAsync(id);
                return File(model.Bytes, "application/octet-stream", $"{model.Name}.stl");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult GetCadsCount()
        {
            int userCads = cadService.Count(c => c.CreatorId == User.GetId());
            int unvCads = cadService.Count(c => c.IsValidated == false);

            return Ok(new CadsCountModel()
            {
                UserCadsCount = userCads,
                UnvalidatedCadsCount = unvCads,
            });
        }

        [HttpPost]
        public async Task<IActionResult> MakeContributor()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            AppUser user = await userManager.FindByIdAsync(User.GetId());
            IEnumerable<string> roles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRoleAsync(user, roles.Single());
            await userManager.AddToRoleAsync(user, "Contributor");

            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Cads", new { area = "Contributor" });
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            string key = CookieRequestCultureProvider.DefaultCookieName;
            string value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));

            Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });

            return LocalRedirect(returnUrl);
        }
        public IActionResult Privacy()
        {
            logger.LogInformation("Entered Privacy Policy Page");
            return View();
        }

        public new IActionResult Unauthorized() => base.Unauthorized();
    }
}