using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models.Cads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.Extensions.Localization;
using CustomCADSolutions.App.Extensions;

namespace CustomCADSolutions.App.Areas.Designer.Controllers
{
    [Area("Designer")]
    [Authorize(Roles = "Designer")]
    public class CadsController : Controller
    {
        private readonly ILogger logger;
        private readonly ICadService cadService;
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IStringLocalizer<CadsController> localizer;

        public CadsController(
            ICadService cadService,
            IOrderService orderService,
            ICategoryService categoryService,
            ILogger<CadModel> logger,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostingEnvironment,
            IStringLocalizer<CadsController> localizer)
        {
            this.cadService = cadService;
            this.orderService = orderService;
            this.categoryService = categoryService;
            this.logger = logger;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
            this.localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] CadQueryInputModel inputQuery)
        {
            ViewBag.ModelsPerPage = 3;
            CadQueryModel query = await cadService.GetAllAsync(
                category: inputQuery.Category,
                creatorName: inputQuery.Creator,
                searchTerm: inputQuery.SearchTerm,
                sorting: inputQuery.Sorting,
                validated: true,
                unvalidated: true,
                currentPage: inputQuery.CurrentPage,
                modelsPerPage: ViewBag.ModelsPerPage);

            inputQuery.TotalCadsCount = query.TotalCount;
            inputQuery.Categories = (await categoryService.GetAllAsync()).Select(c => c.Name);
            inputQuery.Cads = query.CadModels
                .Select(c => new CadViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Category = c.Category.Name,
                    CreationDate = c.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatorName = c.Creator!.UserName,
                    Coords = c.Coords,
                    SpinAxis = c.SpinAxis,
                    SpinFactor = c.SpinFactor,
                    IsValidated = c.IsValidated,
                }).ToArray();

            ViewBag.Sortings = typeof(CadSorting).GetEnumNames();

            int designerModelsCount = (await cadService
                .GetAllAsync(creatorName: User.Identity!.Name))
                .TotalCount;

            ViewBag.DesignerDetails = designerModelsCount > 0 ?
                localizer["Has", designerModelsCount] :
                localizer["Hasn't"];

            int unvalidatedModelsCount = inputQuery.Cads.Count(c => !c.IsValidated);

            ViewBag.UnvalidatedDetails = unvalidatedModelsCount > 0 ?
                localizer["Has", unvalidatedModelsCount] :
                localizer["Hasn't"];

            return View(inputQuery);
        }

        [HttpGet]
        public async Task<IActionResult> Submitted([FromQuery] CadQueryInputModel inputQuery)
        {
            ViewBag.ModelsPerPage = 4;
            CadQueryModel query = await cadService.GetAllAsync(
                category: inputQuery.Category,
                creatorName: inputQuery.Creator,
                validated: false,
                unvalidated: true,
                searchTerm: inputQuery.SearchTerm,
                sorting: inputQuery.Sorting,
                currentPage: inputQuery.CurrentPage,
                modelsPerPage: ViewBag.ModelsPerPage);

            inputQuery.TotalCadsCount = query.TotalCount;
            inputQuery.Categories = (await categoryService.GetAllAsync()).Select(c => c.Name);
            inputQuery.Cads = query.CadModels
                .Select(m => new CadViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Category = m.Category.Name,
                    CreationDate = m.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatorName = m.Creator!.UserName,
                    Coords = m.Coords,
                    SpinAxis = m.SpinAxis,
                    SpinFactor = m.SpinFactor,
                    IsValidated = m.IsValidated,
                }).ToArray();

            ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
            return View(inputQuery);
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
            ViewBag.ModelsPerPage = 8;
            CadQueryModel query = await cadService
                .GetAllAsync(creatorName: User.Identity!.Name, 
                currentPage: inputQuery.CurrentPage,
                modelsPerPage: ViewBag.ModelsPerPage);

            inputQuery.TotalCadsCount = query.TotalCount;
            inputQuery.Cads = query.CadModels
                .Select(model => new CadViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Category = model.Category.Name,
                    CreationDate = model.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatorName = model.Creator!.UserName,
                    Coords = model.Coords,
                    SpinAxis = model.SpinAxis,
                    SpinFactor = model.SpinFactor,
                    IsValidated = model.IsValidated,
                })
                .ToArray();

            return View(inputQuery);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            logger.LogInformation("Entered Submit Page");

            CadInputModel input = new() { Categories = await categoryService.GetAllAsync() };
            return View(input);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(CadInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid 3d Model");
                input.Categories = await categoryService.GetAllAsync();
                if (ModelState.ErrorCount > 1)
                {
                    return View(input);
                }
            }

            if (input.CadFile == null || input.CadFile.Length <= 0)
            {
                return BadRequest("Invalid 3d model");
            }

            CadModel model = new()
            {
                Name = input.Name,
                CategoryId = input.CategoryId,
                IsValidated = User.IsInRole("Designer"),
                CreationDate = DateTime.Now,
                CreatorId = User.GetId()
            };

            int cadId = await cadService.CreateAsync(model);
            await hostingEnvironment.UploadCadAsync(input.CadFile, cadId, input.Name);
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);

            if (model.Creator == null)
            {
                return BadRequest();
            }

            if (model.CreatorId != User.GetId())
            {
                return Unauthorized();
            }

            CadInputModel input = new()
            {
                Categories = await categoryService.GetAllAsync(),
                Name = model.Name,
                CategoryId = model.CategoryId,
                X = model.Coords.Item1,
                Y = model.Coords.Item2,
                Z = model.Coords.Item3,
                SpinAxis = model.SpinAxis,
                SpinFactor = (int)(model.SpinFactor * 100),
            };

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CadInputModel input, int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);

            if (model.Creator == null)
            {
                return BadRequest();
            }

            if (model.CreatorId != User.GetId())
            {
                return Unauthorized();
            }

            if (input.Name != model.Name)
            {
                hostingEnvironment.EditCad(id, model.Name, input.Name);
                model.Name = input.Name;
            }

            model.CategoryId = input.CategoryId;
            model.Coords = (input.X, input.Y, input.Z);
            model.SpinAxis = input.SpinAxis;
            model.SpinFactor = input.SpinFactor / 100d;

            await cadService.EditAsync(model);
            logger.LogInformation("Saved Model Changes");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CadModel cad = await cadService.GetByIdAsync(id);

            if (cad.CreatorId != User.GetId())
            {
                return Unauthorized();
            }

            hostingEnvironment.DeleteCad(cad.Name, cad.Id);

            OrderModel[] orders = (await orderService.GetAllAsync()).Where(o => o.CadId == cad.Id).ToArray();
            foreach (OrderModel order in orders)
            {
                order.Status = OrderStatus.Pending;
            }

            await orderService.EditRangeAsync(orders);
            await cadService.DeleteAsync(cad.Id);

            return RedirectToAction(nameof(Index));
        }
    }
}
