using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CustomCADSolutions.App.Controllers
{
    [Authorize]
    public class CadController : Controller
    {
        private readonly ICadService cadService;
        private readonly ILogger logger;

        public CadController(ICadService service, ILogger<CadModel> logger)
        {
            this.cadService = service;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Categories()
        {
            logger.LogInformation("Entered Categories Page");
            ViewBag.Categories = string.Join(" ", "All", string.Join(" ", typeof(Category).GetEnumNames())).Split();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string category)
        {
            logger.LogInformation("Entered Explore Page");
            ViewBag.Category = category;

            IEnumerable<CadViewModel> views = (await GetCads(category ?? throw new ArgumentNullException()))
                .Select(model => ConvertModelToView(model));

            return View(views);
        }

        [HttpGet]
        public IActionResult Add()
        {
            logger.LogInformation("Entered Submit Page");
            return View(new CadInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(CadInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid 3d Model");
                return View(input);
            }
            
            CadModel model = ConvertInputToModel(input);
            await cadService.CreateAsync(model);

            logger.LogInformation("Submitted 3d Model");
            return RedirectToAction(nameof(Index), routeValues: model.Category);
        }


        private async Task<IEnumerable<CadModel>> GetCads(string category)
        {
            IEnumerable<CadModel> cads = (await cadService.GetAllAsync())
                .Where(c => c.CreationDate.HasValue);

            IEnumerable<string> categories = typeof(Category).GetEnumNames();
            if (categories.Contains(category))
            {
                cads = cads.Where(cad => category == cad.Category.ToString());
            }

            return cads;
        }

        private static CadViewModel ConvertModelToView(CadModel cad)
        {
            int fileLength = cad.CadInBytes!.Length;
            string cadName = cad.Name;
            string fileName = cadName.ToLower();
            using MemoryStream stream = new(cad.CadInBytes);
            FormFile cadFile = new(stream, 0, fileLength, cadName, fileName);

            string cadCategory = cad.Category.ToString();
            string? cadCreationDate = cad.CreationDate?.ToString("dd/MM/yyyy HH:mm:ss");

            CadViewModel view = new()
            {
                Name = cadName,
                Category = cadCategory,
                CreationDate = cadCreationDate,
                Cad = cadFile,
            };

            return view;
        }

        private static CadModel ConvertInputToModel(CadInputModel input)
        {
            using MemoryStream stream = new();
            input.File.CopyTo(stream);
            CadModel model = new()
            {
                Name = input.Name,
                Category = input.Category,
                CreationDate = DateTime.Now,
                CadInBytes = stream.ToArray(),
            };
            return model;
        }
    }
}
