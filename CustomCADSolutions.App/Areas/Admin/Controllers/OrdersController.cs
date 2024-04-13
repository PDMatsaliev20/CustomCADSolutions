using AutoMapper;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Mappings;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.Core.Mappings.DTOs;
using CustomCADSolutions.App.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Administrator")]
    public class OrdersController : Controller
    {
        // Services
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;

        //Addons
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
        public async Task<IActionResult> Index()
        {
            IEnumerable<OrderModel> orders = await orderService.GetAllAsync();
            var views = mapper.Map<OrderViewModel[]>(orders);

            ViewBag.Categories = await categoryService.GetAllAsync();
            return View(views);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await orderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
