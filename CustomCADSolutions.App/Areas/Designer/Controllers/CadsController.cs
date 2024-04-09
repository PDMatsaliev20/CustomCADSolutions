using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models.Cads;
using Microsoft.AspNetCore.Authorization;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.Extensions.Localization;
using CustomCADSolutions.App.Extensions;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.App.Mappings.CadDTOs;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.App.Areas.Designer.Controllers
{
    [Area("Designer")]
    [Authorize(Roles = "Designer")]
    public class CadsController : Controller
    {
        private readonly IStringLocalizer<CadsController> localizer;
        private readonly ILogger<CadsController> logger;
        private readonly ICadService cadService;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public CadsController(
            ICadService cadService,
            IStringLocalizer<CadsController> localizer,
            ILogger<CadsController> logger,
            HttpClient httpClient)
        {
            this.cadService = cadService;
            this.localizer = localizer;
            this.httpClient = httpClient;
            MapperConfiguration config = new(cfg => cfg.AddProfile<CadDTOProfile>());
            mapper = config.CreateMapper();
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] CadQueryInputModel inputQuery)
        {
            Dictionary<string, string> parameters = new()
            {
                ["validated"] = "true",
                ["unvalidated"] = "true",
            };
            try
            {
                string _ = CadsAPIPath + HttpContext.SecureQuery(parameters.ToArray());
                var query = (await httpClient.GetFromJsonAsync<CadQueryDTO>(_))!;
                
                _ = CategoriesAPIPath;
                var categories = (await httpClient.GetFromJsonAsync<Category[]>(_))!;

                inputQuery.Categories = categories.Select(c => c.Name);
                inputQuery.TotalCount = query.TotalCount;
                inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads);

                parameters["creator"] = User.Identity!.Name!;
                ViewBag.DesignerDetails = await GetMessageAsync(parameters.ToArray());

                parameters = new() { ["unvalidated"] = "true", };
                ViewBag.UnvalidatedDetails = await GetMessageAsync(parameters.ToArray());

                ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
                return View(inputQuery);
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Submitted([FromQuery] CadQueryInputModel inputQuery)
        {
            Dictionary<string, string> parameters = new()
            {
                ["unvalidated"] = "true",
            };
            string path = CadsAPIPath + HttpContext.SecureQuery(parameters.ToArray());
            var query = await httpClient.GetFromJsonAsync<CadQueryDTO>(path);
            if (query != null)
            {
                var categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath);
                if (categories == null)
                {
                    return NotFound();
                }

                inputQuery.Categories = categories.Select(c => c.Name);
                inputQuery.TotalCount = query.TotalCount;
                inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads);

                ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
                return View(inputQuery);
            }
            else return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ValidateCad(int cadId)
        {
            CadModel model = await cadService.GetByIdAsync(cadId);

            model.IsValidated = true;
            await cadService.EditAsync(model);

            return RedirectToAction(nameof(Submitted));
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CadQueryInputModel inputQuery)
        {
            Dictionary<string, string> parameters = new()
            {
                ["validated"] = "true",
                ["unvalidated"] = "true",
                ["creator"] = User.Identity!.Name!,
            };
            string path = CadsAPIPath + HttpContext.SecureQuery(parameters.ToArray());
            var query = await httpClient.GetFromJsonAsync<CadQueryDTO>(path);
            if (query != null)
            {
                var categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath);
                if (categories == null)
                {
                    return NotFound();
                }

                inputQuery.TotalCount = query.TotalCount;
                inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads);
                inputQuery.Categories = categories.Select(c => c.Name);

                return View(inputQuery);
            }
            else return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new CadInputModel()
            {
                Categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath)
            });
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Add(CadInputModel input)
        {
            int maxFileSize = 10_000_000;
            if (input.CadFile.Length > maxFileSize)
            {
                ModelState.AddModelError(nameof(input.CadFile),
                    $"3d model cannot be over {maxFileSize / 1_000_000}MB");
            }

            if (!ModelState.IsValid)
            {
                input.Categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath);
                if (ModelState.ErrorCount > 1)
                {
                    return View(input);
                }
            }

            if (input.CadFile == null || input.CadFile.Length <= 0)
            {
                return BadRequest("Invalid 3d model");
            }

            CadImportDTO dto = mapper.Map<CadImportDTO>(input);
            dto.Bytes = await input.CadFile.GetBytesAsync();
            dto.CreatorId = User.GetId();
            dto.IsValidated = true;

            var response = await httpClient.PostAsJsonAsync(CadsAPIPath, dto);
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await httpClient.GetFromJsonAsync<CadExportDTO>($"{CadsAPIPath}/{id}");
            if (dto != null)
            {
                if (dto.CreatorName != User.Identity!.Name!)
                {
                    return Forbid();
                }

                string path = $"{CategoriesAPIPath}/{dto.CategoryId}";
                var category = await httpClient.GetFromJsonAsync<Category>(path);
                if (category == null)
                {
                    return NotFound();
                }

                CadInputModel input = new()
                {
                    Name = dto.Name,
                    X = dto.Coords[0],
                    Y = dto.Coords[1],
                    Z = dto.Coords[2],
                    SpinAxis = dto.SpinAxis,
                    Price = dto.Price,
                    CategoryId = category.Id,
                    Categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath),
                };

                return View(input);
            }
            else return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CadInputModel input, int id)
        {
            var dto = mapper.Map<CadImportDTO>(input);
            dto.CreatorId = User.GetId();
            var response = await httpClient.PutAsJsonAsync(CadsAPIPath, dto);

            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await httpClient.GetFromJsonAsync<CadExportDTO>($"{CadsAPIPath}/{id}");
            if (dto != null)
            {
                if (dto.CreatorName != User.Identity!.Name)
                {
                    return Forbid();
                }

                await httpClient.DeleteAsync($"{CadsAPIPath}/{id}");
                return RedirectToAction(nameof(Index));
            }
            else return BadRequest();
        }

        private async Task<string> GetMessageAsync(params KeyValuePair<string, string>[] parameters)
        {
            string path = CadsAPIPath + HttpContext.SecureQuery(parameters);
            var query = await httpClient.GetFromJsonAsync<CadQueryDTO>(path);

            if (query != null && query.TotalCount > 0)
            {
                return localizer["Has", query.TotalCount];
            }
            else return localizer["Hasn't"];

        }

    }
}
