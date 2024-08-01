using AutoMapper;
using CustomCADs.App.Extensions;
using CustomCADs.App.Mappings;
using CustomCADs.App.Models.Cads.View;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Domain.Entities.Enums;
using CustomCADs.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using static CustomCADs.App.Extensions.UtilityExtensions;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.App.Controllers
{
    public class HomeController(
        ICadService cadService,
        ICategoryService categoryService,
        IOrderService orderService,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment env,
        ILogger<HomeController> logger) : Controller
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg 
                => cfg.AddProfile<CadAppProfile>())
            .CreateMapper();

        [HttpGet]
        public IActionResult Index()
        {
            return View(new CadViewModel()
            {
                Id = 0,
                Name = "Laptop",
                Extension = ".glb",
                Coords = [23, 15, 20],
                PanCoords = [0, 6, 0]
            });
        }

        [HttpGet]
        public async Task<IActionResult> Category([FromQuery] CadQueryInputModel query)
        {
            CadQueryResult result = await cadService.GetAllAsync(new()
            {
                Category = query.Category,
                Creator = query.Creator,
                SearchName = query.SearchName,
                SearchCreator = query.SearchCreator,
                Sorting = query.Sorting,
                CurrentPage = query.CurrentPage,
                CadsPerPage = query.CadsPerPage,
                Status = CadStatus.Validated,
            });

            query.Categories = await categoryService.GetAllNamesAsync();
            query.TotalCount = result.Count;
            query.Cads = mapper.Map<CadViewModel[]>(result.Cads);

            ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
            ViewBag.Category = query.Category;
            return View(query);
        }

        [HttpGet]
        public async Task<ActionResult> DownloadCad(int id)
        {
            try
            {
                CadModel model = await cadService.GetByIdAsync(id);
                IEnumerable<OrderModel> orders = (await orderService.GetAllAsync())
                    .Where(o => o.CadId == id);

                bool hasPermission =
                    orders.Select(o => o.BuyerId).Contains(User.GetId())
                    || model.CreatorId == User.GetId();

                if (!hasPermission)
                {
                    return StatusCode(402);
                }

                if (!model.IsFolder)
                {
                    string downloadName = $"{model.Name}{model.CadExtension}";
                    return File(model.CadPath, "application/octet-stream", downloadName);
                }
                else
                {
                    string path = Path.Combine(env.WebRootPath, "others", "cads", model.Name + model.Id);
                    if (!Directory.Exists(path))
                    {
                        return NotFound();
                    }

                    string temp = Path.GetTempFileName();
                    try
                    {
                        using (FileStream zipToOpen = new(temp, FileMode.OpenOrCreate))
                        {
                            using ZipArchive archive = new(zipToOpen, ZipArchiveMode.Create, true);

                            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                            foreach (string file in files)
                            {
                                string entryName = Path.GetRelativePath(path, file);
                                archive.CreateEntryFromFile(file, entryName);
                            }
                        }
                        FileStream stream = new(temp, FileMode.Open, FileAccess.Read, FileShare.None);
                        return File(stream, "application/zip", $"{model.Name}.zip");
                    }
                    catch (Exception ex)
                    {
                        if (System.IO.File.Exists(temp))
                        {
                            System.IO.File.Delete(temp);
                        }

                        return StatusCode(500, ex.Message);
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> MakeContributor()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            try
            {
                AppUser user = await userManager.FindByIdAsync(User.GetId())
                ?? throw new KeyNotFoundException();
                IEnumerable<string> roles = await userManager.GetRolesAsync(user);

                await userManager.RemoveFromRoleAsync(user, roles.Single());
                await userManager.AddToRoleAsync(user, Contributor);

                await signInManager.SignOutAsync();
                await signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Cads", new { area = "Contributor" });
            }
            catch (KeyNotFoundException)
            {
                return Unauthorized();
            }
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

        public IActionResult Privacy() => View();

        public new IActionResult Unauthorized() => base.Unauthorized();
    }
}