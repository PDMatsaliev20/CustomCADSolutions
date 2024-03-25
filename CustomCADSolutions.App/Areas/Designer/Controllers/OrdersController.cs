using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using CustomCADSolutions.App.Extensions;

namespace CustomCADSolutions.App.Areas.Designer.Controllers
{
    [Area("Designer")]
    [Authorize(Roles = "Designer")]
    public class OrdersController : Controller
    {
        private readonly ICadService cadService;
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public OrdersController(
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
        public async Task<IActionResult> All()
        {
            ViewBag.Statuses = typeof(OrderStatus).GetEnumNames();
            
            IEnumerable<OrderModel> models = await orderService.GetAllAsync();
            ViewBag.HiddenOrders = models.Count(m => !m.ShouldShow);

            OrderViewModel[] orders = models
                .Where(m => m.ShouldShow)
                .OrderBy(m => m.OrderDate)
                .Select(m => new OrderViewModel
                {
                    BuyerId = m.BuyerId,
                    BuyerName = m.Buyer.UserName,
                    CadId = m.CadId,
                    Category = m.Cad.Category.Name,
                    Name = m.Cad.Name,
                    Description = m.Description,
                    Status = m.Status.ToString(),
                    OrderDate = m.OrderDate.ToString("dd/MM/yyyy"),
                })
                .ToArray();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Begin(CadInputModel input)
        {
            OrderModel? model = await orderService.GetByIdAsync(input.Id, input.BuyerId!);
            if (model == null)
            {
                return BadRequest();
            }

            model.Status = OrderStatus.Begun;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Finish(CadInputModel input)
        {
            OrderModel? model = await orderService.GetByIdAsync(input.Id, input.BuyerId!);

            if (model == null)
            {
                return BadRequest();
            }

            await hostingEnvironment.UploadCadAsync(input.CadFile, input.Id, model.Cad.Name);

            model.Cad.CreatorId = User.GetId();
            model.Cad.CreationDate = DateTime.Now;
            model.Cad.IsValidated = true;

            model.Status = OrderStatus.Finished;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Hide(CadInputModel input)
        {
            OrderModel? model = await orderService.GetByIdAsync(input.Id, input.BuyerId!);

            if (model == null)
            {
                return BadRequest();
            }

            model.ShouldShow = false;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }
    }
}
