using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.App.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(RoleConstants.Admin)]
    public class OrdersController(
        IOrderService orderService,
        ICategoryService categoryService,
        ILogger<OrdersController> logger) : Controller
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg
                    => cfg.AddProfile<OrderAppProfile>())
                .CreateMapper();

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
