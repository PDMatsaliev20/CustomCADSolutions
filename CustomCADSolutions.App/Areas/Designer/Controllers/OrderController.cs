using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomCADSolutions.App.Areas.Designer.Controllers
{
    [Area("Designer")]
    [Authorize(Roles = "Designer")]
    public class OrderController : Controller
    {
        private readonly ICadService cadService;
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public OrderController(
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
            logger.LogInformation("Entered All Orders Page");

            IEnumerable<OrderModel> models = await orderService.GetAllAsync();
            
            ViewBag.Statuses = typeof(OrderStatus).GetEnumNames();
            ViewBag.BgStatuses = "Предстояща Започната Завършена".Split();

            ViewBag.HiddenOrders = models.Count(m => !m.ShouldShow);
            ViewBag.Orders = models
                .Where(m => m.ShouldShow)
                .OrderBy(m => m.OrderDate)
                .Select(m => new OrderViewModel
                {
                    BuyerId = m.BuyerId,
                    BuyerName = m.Buyer.UserName,
                    CadId = m.CadId,
                    Category = m.Cad.Category.Name,
                    BgCategory = m.Cad.Category.BgName,
                    Name = m.Cad.Name,
                    Description = m.Description,
                    Status = m.Status.ToString(),
                    OrderDate = m.OrderDate.ToString("dd/MM/yyyy"),
                });

            return View(new CadInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> BeginOrder(CadInputModel input)
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
        public async Task<IActionResult> FinishOrder(CadInputModel input)
        {
            OrderModel? model = await orderService.GetByIdAsync(input.Id, input.BuyerId!);

            if (model == null)
            {
                return BadRequest();
            }

            await UploadFileAsync(input.CadFile, input.Id, model.Cad.Name);

            model.Cad.CreatorId = GetUserId();
            model.Cad.CreationDate = DateTime.Now;
            model.Cad.Validated = true;

            model.Status = OrderStatus.Finished;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> HideOrder(CadInputModel input)
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

        // Private methods

        private static async Task UploadFileAsync(IFormFile cad, int cadId, string cadName, string extension = ".stl")
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/others/cads/{cadName}{cadId}{extension}");
            using FileStream fileStream = new(filePath, FileMode.Create);
            await cad.CopyToAsync(fileStream);
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
