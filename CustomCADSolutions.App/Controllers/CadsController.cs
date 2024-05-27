using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.App.Models.Cads.Input;
using CustomCADSolutions.App.Models.Cads.View;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using static CustomCADSolutions.Infrastructure.Data.DataConstants.RoleConstants;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Hubs;
using Stripe;
using NuGet.Packaging.Signing;
using System.Text.RegularExpressions;

namespace CustomCADSolutions.App.Controllers
{
    [Authorize(Roles = $"{Contributor},{Designer}")]
    public class CadsController : Controller
    {
        // Services
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;
        private readonly CadsHubHelper statisticsService;

        // Addons
        private readonly IWebHostEnvironment env;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public CadsController(
            ICadService cadService,
            ICategoryService categoryService,
            CadsHubHelper statisticsService,
            IWebHostEnvironment env,
            ILogger<CadsController> logger)
        {
            this.cadService = cadService;
            this.categoryService = categoryService;
            this.statisticsService = statisticsService;

            MapperConfiguration config = new(cfg => cfg.AddProfile<CadAppProfile>());
            this.mapper = config.CreateMapper();
            this.env = env;
            this.logger = logger;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateCoords(int id, int x, int y, int z)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            model.Coords = [x, y, z];

            await cadService.EditAsync(id, model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CadQueryInputModel inputQuery)
        {
            // Ensuring cads per page are divisible by the count of columns
            if (inputQuery.CadsPerPage % inputQuery.Cols != 0)
            {
                inputQuery.CadsPerPage = inputQuery.Cols * (inputQuery.CadsPerPage / inputQuery.Cols);
            }

            CadQueryModel query = new()
            {
                Category = inputQuery.Category,
                SearchName = inputQuery.SearchName,
                Sorting = inputQuery.Sorting,
                CurrentPage = inputQuery.CurrentPage,
                CadsPerPage = inputQuery.CadsPerPage,
                Creator = User.Identity!.Name,
                Validated = true,
                Unvalidated = true,
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
        public async Task<IActionResult> Details(int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            return View(mapper.Map<CadViewModel>(model));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new CadAddModel()
            {
                Categories = await categoryService.GetAllAsync()
            });
        }

        [HttpPost]
        [RequestSizeLimit(150_000_000)]
        public async Task<IActionResult> Add(CadAddModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Categories = await categoryService.GetAllAsync();
                return View(input);
            }

            CadModel model = mapper.Map<CadModel>(input);
            model.CreatorId = User.GetId();
            model.IsValidated = User.IsInRole(Designer);
            model.CreationDate = DateTime.Now;

            if (input.CadFolder != null)
            {
                string[] cadFormats = [".gltf", ".glb"];

                IFormFile cad = input.CadFolder
                    .Single(f => cadFormats.Contains(f.GetFileExtension()));

                Regex regex = new(@"^\w+/");
                string partToRemove = regex.Match(cad.FileName).Value;
                string cadPath = cad.FileName[partToRemove.Length..];

                model.Extension = cad.GetFileExtension();
                int cadId = await cadService.CreateAsync(model);

                string directory = env.GetFilePath(input.Name + cadId);
                Directory.CreateDirectory(directory);

                string fullCadPath = Path.Combine(directory, cadPath);
                using FileStream cadStream = new(fullCadPath, FileMode.Create);
                await cad.CopyToAsync(cadStream);

                foreach (IFormFile file in input.CadFolder.Where(f => f != cad))
                {
                    partToRemove = regex.Match(file.FileName).Value;
                    string filePath = file.FileName[partToRemove.Length..];

                    Regex newRegex = new(@"/?\w+.\w+$");
                    string fileName = newRegex.Match(filePath).Value;

                    string folders = filePath.Substring(0, filePath.Length - fileName.Length);
                    if (!string.IsNullOrWhiteSpace(folders))
                    {
                        Directory.CreateDirectory(Path.Combine(directory, folders));
                    }

                    string fullFilePath = Path.Combine(directory, filePath);
                    using FileStream stream = new(fullFilePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }
            else if (input.CadFile != null)
            {
                model.Extension = input.CadFile.GetFileExtension();
                int cadId = await cadService.CreateAsync(model);
                await env.UploadFileAsync(input.CadFile, input.Name + cadId + input.CadFile.GetFileExtension());
            }
            else return BadRequest();

            await statisticsService.SendStatistics(User.GetId());
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            if (model.CreatorId != User.GetId())
            {
                return Forbid("You don't have access to this model");
            }

            CadEditModel input = mapper.Map<CadEditModel>(model);
            input.Categories = await categoryService.GetAllAsync();

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CadEditModel input)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            model.Name = input.Name;
            model.CategoryId = input.CategoryId;
            model.Price = input.Price;

            await cadService.EditAsync(id, model);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CadModel cad = await cadService.GetByIdAsync(id);

            env.DeleteFile(cad.Name + cad.Id, cad.Extension);
            await cadService.DeleteAsync(id);
            await statisticsService.SendStatistics(User.GetId());

            return RedirectToAction(nameof(Index));
        }
    }
}
