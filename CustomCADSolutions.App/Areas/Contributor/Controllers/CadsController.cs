using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CustomCADSolutions.App.Extensions;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.App.Mappings.CadDTOs;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.App.Models.Cads.Input;
using CustomCADSolutions.App.Models.Cads.View;
using System.Drawing;

namespace CustomCADSolutions.App.Areas.Contributor.Controllers
{
    [Area("Contributor")]
    [Authorize(Roles = "Contributor")]
    public class CadsController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public CadsController(HttpClient httpClient, ILogger<CadsController> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            MapperConfiguration config = new(cfg => cfg.AddProfile<CadDTOProfile>());
            this.mapper = config.CreateMapper();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeColor(int id, string colorParam)
        {
            string _;
            try
            {
                _ = $"{CadsAPIPath}/{id}";
                var export = (await httpClient.GetFromJsonAsync<CadExportDTO>(_))!;

                Color color = ColorTranslator.FromHtml(colorParam);
                CadImportDTO import = new()
                {
                    Id = export.Id,
                    CreatorId = export.CreatorId,
                    Name = export.Name,
                    Coords = export.Coords,
                    SpinAxis = export.SpinAxis,
                    RGB = new byte[] { color.R, color.G, color.B },
                    CategoryId = export.CategoryId,
                    Price = export.Price,
                };

                var response = await httpClient.PutAsJsonAsync(CadsAPIPath, import);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Details), new { id = export.Id });
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
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
            CadQueryDTO? query = await httpClient.GetFromJsonAsync<CadQueryDTO>(path);
            if (query != null)
            {
                inputQuery.TotalCount = query.TotalCount;
                inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads.ToArray());

                return View(inputQuery);
            }
            else return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string _;
            try
            {
                _ = $"{CadsAPIPath}/{id}";
                var dto = await httpClient.GetFromJsonAsync<CadExportDTO>(_);
                return View(mapper.Map<CadViewModel>(dto));
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new CadAddModel()
            {
                Categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath)
            });
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Add(CadAddModel input)
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
                return View(input);
            }

            if (input.CadFile == null || input.CadFile.Length <= 0)
            {
                return BadRequest("Invalid 3d model");
            }

            CadImportDTO dto = mapper.Map<CadImportDTO>(input);
            dto.Bytes = await input.CadFile.GetBytesAsync();
            dto.CreatorId = User.GetId();
            dto.IsValidated = false;

            var response = await httpClient.PostAsJsonAsync(CadsAPIPath, dto);
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string _ = $"{CadsAPIPath}/{id}";
            try
            {
                var dto = (await httpClient.GetFromJsonAsync<CadExportDTO>(_))!;
                if (dto.CreatorId == null)
                {
                    return BadRequest("Model hasn't been created yet");
                }

                if (dto.CreatorId != User.GetId())
                {
                    return Forbid("You don't have access to this model");
                }

                CadEditModel input = new()
                {
                    Id = id,
                    Name = dto.Name,
                    Price = dto.Price,
                    SpinAxis = dto.SpinAxis,
                    X = dto.Coords[0],
                    Y = dto.Coords[1],
                    Z = dto.Coords[2],
                    CategoryId = dto.CategoryId,
                    Categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath),
                };

                return View(input);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CadEditModel input)
        {
            CadImportDTO dto = mapper.Map<CadImportDTO>(input);
            dto.CreatorId = User.GetId();

            var response = await httpClient.PutAsJsonAsync(CadsAPIPath, dto);
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await httpClient.DeleteAsync($"{CadsAPIPath}/{id}");
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Index));
        }
    }
}
