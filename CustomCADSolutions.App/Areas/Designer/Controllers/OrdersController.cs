using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.Core.Mappings.DTOs;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Core.Mappings.CadDTOs;
using CustomCADSolutions.App.Models.Cads.Input;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Core.Services;

namespace CustomCADSolutions.App.Areas.Designer.Controllers
{
    [Area("Designer")]
    [Authorize(Roles = "Designer")]
    public class OrdersController : Controller
    {
        // Services
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;

        // Addons
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public OrdersController(
            IOrderService orderService,
            ICategoryService categoryService,
            ILogger<OrdersController> logger)
        {
            this.orderService = orderService;
            this.categoryService = categoryService;

            this.logger = logger;
            MapperConfiguration config = new(cfg => cfg.AddProfile<OrderDTOProfile>());
            this.mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<OrderModel> orders = await orderService.GetAllAsync();
            var views = mapper.Map<OrderViewModel[]>(orders);

            ViewBag.Statuses = typeof(OrderStatus).GetEnumNames();
            return View(views);
        }

        [HttpPost]
        public async Task<IActionResult> Begin(int id)
        {
            OrderModel model = await orderService.GetByIdAsync(id);
            model.Status = OrderStatus.Begun;

            await orderService.EditAsync(id, model);
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Finish(int id)
        {
            OrderModel model = await orderService.GetByIdAsync(id);

            CadFinishModel input = new()
            {
                OrderId = id,
                Description = model.Description,
                Categories = await categoryService.GetAllAsync()
            };
            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Finish(int id, CadFinishModel cad)
        {
            OrderModel model = await orderService.GetByIdAsync(id);
            model.Cad = new()
            {
                Name = cad.Name,
                Price = cad.Price,
                CategoryId = cad.CategoryId,
                Bytes = await cad.CadFile.GetBytesAsync(),
                CreatorId = User.GetId(),
                IsValidated = true,
            };

            await orderService.FinishOrderAsync(id, model);
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Hide(int id)
        {
            OrderModel model = await orderService.GetByIdAsync(id);
            model.ShouldShow = false;

            await orderService.EditAsync(id, model);
            return RedirectToAction(nameof(All));
        }
    }
}
