using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models.Cads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using CustomCADSolutions.App.Extensions;

namespace CustomCADSolutions.App.Areas.Contributer.Controllers
{
    [Area("Contributer")]
    [Authorize(Roles = "Contributer")]
    public class CadsController : Controller
    {
        private readonly ICadService cadService;
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public CadsController(
            ICadService cadService,
            IOrderService orderService,
            ICategoryService categoryService,
            ILogger<CadModel> logger,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostingEnvironment)
        {
            this.cadService = cadService;
            this.orderService = orderService;
            this.categoryService = categoryService;
            this.logger = logger;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CadQueryInputModel inputQuery)
        {
            ViewBag.ModelsPerPage = 8;
            CadQueryModel query = await cadService
                .GetAllAsync(category: inputQuery.Category,
                    creatorName: User.Identity!.Name, 
                    sorting: CadSorting.Oldest,
                    searchTerm: inputQuery.SearchTerm,
                    currentPage: inputQuery.CurrentPage,
                    modelsPerPage: ViewBag.ModelsPerPage);

            inputQuery.Categories = (await categoryService.GetAllAsync()).Select(c => c.Name);
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
            await hostingEnvironment.UploadCadAsync(input.CadFile, cadId, model.Name);

            logger.LogInformation("Submitted 3d Model");
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
