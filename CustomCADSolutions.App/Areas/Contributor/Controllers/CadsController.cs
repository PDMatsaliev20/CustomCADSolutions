using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models.Cads;
using Microsoft.AspNetCore.Authorization;
using CustomCADSolutions.App.Extensions;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.App.Mappings.CadDTOs;
using AutoMapper;
using CustomCADSolutions.App.Mappings;

namespace CustomCADSolutions.App.Areas.Contributor.Controllers
{
    [Area("Contributor")]
    [Authorize(Roles = "Contributor")]
    public class CadsController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public CadsController(
            ICategoryService categoryService,
            ILogger<CadModel> logger,
            HttpClient httpClient)
        {
            this.categoryService = categoryService;
            this.httpClient = httpClient;
            this.logger = logger;
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
            CadQueryDTO? query = await httpClient.TryGetFromJsonAsync<CadQueryDTO>(path);
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
                Categories = await categoryService.GetAllAsync()
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
                logger.LogError("Invalid 3d Model: {0}", string.Join(", ", ModelState.GetErrors()));
                input.Categories = await categoryService.GetAllAsync();
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
            var response = await httpClient.PostAsJsonAsync($"{CadsAPIPath}/Create", dto);
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
            var dto = await httpClient.TryGetFromJsonAsync<CadExportDTO>($"{CadsAPIPath}/{id}");
            if (dto != null)
            {
                if (dto.CreatorName == null)
                {
                    return BadRequest("Model hasn't been created yet");
                }

                if (dto.CreatorName != User.Identity!.Name)
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
                    CategoryId = (await categoryService.GetByNameAsync(dto.CategoryName)).Id,
                    Categories = await categoryService.GetAllAsync(),
                };

                return View(input);
            }
            else return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CadInputModel input, int id)
        {
            CadImportDTO dto = mapper.Map<CadImportDTO>(input);
            dto.CreatorId = User.GetId();

            var response = await httpClient.PutAsJsonAsync($"{CadsAPIPath}/Edit", dto);
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
