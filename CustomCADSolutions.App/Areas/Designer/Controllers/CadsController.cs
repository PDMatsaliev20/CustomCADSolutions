using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models.Cads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.Extensions.Localization;
using CustomCADSolutions.App.Extensions;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;

namespace CustomCADSolutions.App.Areas.Designer.Controllers
{
    [Area("Designer")]
    [Authorize(Roles = "Designer")]
    public class CadsController : Controller
    {
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
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostingEnvironment,
            IStringLocalizer<CadsController> localizer)
        {
            this.cadService = cadService;
            this.orderService = orderService;
            this.categoryService = categoryService;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
            this.localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] CadQueryInputModel inputQuery)
        {
            inputQuery.Cols = 3;
            inputQuery.CadsPerPage = inputQuery.CadsPerPage == 4 ? 3 : inputQuery.CadsPerPage;
            inputQuery = await cadService.QueryCads(inputQuery, true, true);
            inputQuery.Categories = (await categoryService.GetAllAsync()).Select(c => c.Name);
            ViewBag.Sortings = typeof(CadSorting).GetEnumNames();

            int designerModelsCount = (await cadService
                .GetAllAsync(creatorName: User.Identity!.Name))
                .TotalCount;

            ViewBag.DesignerDetails = designerModelsCount > 0 ?
                localizer["Has", designerModelsCount] :
                localizer["Hasn't"];

            int unvalidatedModelsCount = (await cadService
                .GetAllAsync(validated: false, unvalidated: true))
                .TotalCount;

            ViewBag.UnvalidatedDetails = unvalidatedModelsCount > 0 ?
                localizer["Has", unvalidatedModelsCount] :
                localizer["Hasn't"];

            return View(inputQuery);
        }

        [HttpGet]
        public async Task<IActionResult> Submitted([FromQuery] CadQueryInputModel inputQuery)
        {
            inputQuery = await cadService.QueryCads(inputQuery, false, true);
            inputQuery.Categories = (await categoryService.GetAllAsync()).Select(c => c.Name);
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
            inputQuery.Creator = User.Identity!.Name;
            inputQuery = await cadService.QueryCads(inputQuery, true, false);
            inputQuery.Categories = (await categoryService.GetAllAsync()).Select(c => c.Name);

            return View(inputQuery);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            CadInputModel input = new() { Categories = await categoryService.GetAllAsync() };
            return View(input);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(CadInputModel input)
        {
            if (!ModelState.IsValid)
            {
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

            byte[] bytes = await GetBytesFromCadAsync(input.CadFile);
            CadModel model = new()
            {
                Bytes = bytes,
                Name = input.Name,
                CategoryId = input.CategoryId,
                IsValidated = User.IsInRole("Designer"),
                CreationDate = DateTime.Now,
                CreatorId = User.GetId()
            };

            int cadId = await cadService.CreateAsync(model);
            
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

            model.Name = input.Name;
            model.CategoryId = input.CategoryId;
            model.Coords = (input.X, input.Y, input.Z);
            model.SpinAxis = input.SpinAxis;
            model.SpinFactor = input.SpinFactor / 100d;

            await cadService.EditAsync(model);

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
