using AutoMapper;
using CustomCADs.App.Mappings;
using CustomCADs.App.Models.Orders;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.App.Areas.Admin.Controllers
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
