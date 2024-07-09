using CustomCADs.App.Models.Orders;
using static CustomCADs.Infrastructure.Data.DataConstants;
using CustomCADs.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADs.App.Extensions.UtilityExtensions;
using CustomCADs.App.Extensions;
using AutoMapper;
using CustomCADs.App.Mappings;
using CustomCADs.App.Models.Cads.Input;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;

namespace CustomCADs.App.Areas.Designer.Controllers
{
    [Area("Designer")]
    [Authorize(Roles = RoleConstants.Designer)]
    public class OrdersController(
        IOrderService orderService,
        ICategoryService categoryService,
        IWebHostEnvironment env,
        ILogger<OrdersController> logger) : Controller
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg 
                => cfg.AddProfile<OrderAppProfile>())
            .CreateMapper();
        
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
                Extension = cad.CadFile.GetFileExtension(),
                IsValidated = true,
                CreatorId = User.GetId(),
                CreationDate = DateTime.Now,
            };

            int cadId = await orderService.FinishOrderAsync(id, model);
            await env.UploadCadAsync(cad.CadFile, cad.Name + cadId + cad.CadFile.GetFileExtension());

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
