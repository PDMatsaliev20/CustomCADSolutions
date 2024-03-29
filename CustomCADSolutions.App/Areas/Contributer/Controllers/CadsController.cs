using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models.Cads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using CustomCADSolutions.App.Extensions;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;

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
            inputQuery.Creator = User.Identity!.Name;
            inputQuery = await cadService.QueryCads(inputQuery, true, true);
            inputQuery.Categories = (await categoryService.GetAllAsync()).Select(c => c.Name);

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

            byte[] bytes = await GetBytesFromCadAsync(input.CadFile);
            
            CadModel model = new()
            {
                Bytes = bytes,
                Name = input.Name,
                CategoryId = input.CategoryId,
                IsValidated = false,
                CreationDate = DateTime.Now,
                CreatorId = User.GetId()
            };
            int cadId = await cadService.CreateAsync(model);


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
            };

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CadInputModel input, int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);

            if (model.Bytes == null)
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

            await cadService.EditAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CadModel cad = await cadService.GetByIdAsync(id);

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
