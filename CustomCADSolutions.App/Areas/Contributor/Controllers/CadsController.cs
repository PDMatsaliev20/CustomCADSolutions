using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models.Cads;
using Microsoft.AspNetCore.Authorization;
using CustomCADSolutions.App.Extensions;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.App.Mappings.CadDTOs;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.App.Areas.Contributor.Controllers
{
    [Area("Contributor")]
    [Authorize(Roles = "Contributor")]
    public class CadsController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public CadsController(HttpClient httpClient, ILogger<CadsController> logger)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            MapperConfiguration config = new(cfg => cfg.AddProfile<CadDTOProfile>());
            this.mapper = config.CreateMapper();
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
                return View(input);
            }

            if (input.CadFile == null || input.CadFile.Length <= 0)
            {
                return BadRequest("Invalid 3d model");
            }

            byte[] bytes = await input.CadFile.GetBytesAsync();

            CadImportDTO dto = mapper.Map<CadImportDTO>(input);
            dto.Bytes = bytes;
            dto.IsValidated = false;
            dto.CreatorId = User.GetId();
            var response = await httpClient.PostAsJsonAsync(CadsAPIPath, dto);
            response.EnsureSuccessStatusCode();

            //CadModel model = new()
            //{
            //    Bytes = bytes,
            //    Name = input.Name,
            //    CategoryId = input.CategoryId,
            //    IsValidated = false,
            //    CreationDate = DateTime.Now,
            //    CreatorId = User.GetId()
            //};
            //int cadId = await cadService.CreateAsync(model);

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

                CadInputModel input = new()
                {
                    Name = dto.Name,
                    X = dto.Coords[0],
                    Y = dto.Coords[1],
                    Z = dto.Coords[2],
                    SpinAxis = dto.SpinAxis,
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
        public async Task<IActionResult> Edit(CadInputModel input, int id)
        {
            CadImportDTO dto = mapper.Map<CadImportDTO>(input);
            dto.CreatorId = User.GetId();

            var response = await httpClient.PutAsJsonAsync(CadsAPIPath, dto);
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Index));
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
