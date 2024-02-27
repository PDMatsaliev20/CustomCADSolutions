using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomCADSolutions.App.Controllers
{
    [Authorize(Roles = "Client,Contributer,Designer")]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;

        public OrderController(
            ILogger<OrderController> logger,
            IOrderService orderService,
            ICategoryService categoryService,
            UserManager<IdentityUser> userManager)
        {
            this.orderService = orderService;
            this.logger = logger;
            this.userManager = userManager;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation("Entered Orders Page");

            IEnumerable<OrderViewModel> orders = (await orderService.GetAllAsync())
                .Where(o => o.BuyerId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .OrderByDescending(o => (int)o.Status)
                    .ThenBy(o => o.OrderDate)
                .Select(m => new OrderViewModel
                {
                    BuyerId = m.BuyerId,
                    BuyerName = m.Buyer.UserName,
                    CadId = m.CadId,
                    Category = m.Cad.Category.Name,
                    Name = m.Cad.Name,
                    Description = m.Description,
                    Status = m.Status.ToString(),
                    OrderDate = m.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"),
                });

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            logger.LogInformation("Entered Order Page");

            OrderInputModel input = new() { Categories = await GetCategoriesAsync() };
            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order");
                input.Categories = await GetCategoriesAsync();
                return View(input);
            }

            OrderModel model = new()
            {
                Description = input.Description,
                OrderDate = DateTime.Now,
                Status = input.Status,
                ShouldShow = true,
                Buyer = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Cad = new CadModel()
                {
                    Name = input.Name,
                    CategoryId = input.CategoryId,
                }
            };
            await orderService.CreateAsync(model);

            logger.LogInformation("Ordered 3d model");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int cadId)
        {
            logger.LogInformation("Entered Edit Order Page");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                OrderModel model = await orderService.GetByIdAsync(cadId, userId);

                OrderInputModel input = new()
                {
                    Categories = await GetCategoriesAsync(),
                    Status = model.Status,
                    CadId = model.CadId,
                    Name = model.Cad.Name,
                    CategoryId = model.Cad.CategoryId,
                    Description = model.Description,
                    OrderDate = model.OrderDate,
                };

                return View(input);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderInputModel input, int cadId)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order");
                return View(input);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            OrderModel order = await orderService.GetByIdAsync(cadId, userId);

            order.Cad.Name = input.Name;
            order.Description = input.Description;
            order.Cad.CategoryId = input.CategoryId;
            await orderService.EditAsync(order);

            logger.LogInformation("Edited Order");
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Designer")]
        [HttpPost]
        public async Task<IActionResult> Delete(int cadId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await orderService.DeleteAsync(cadId, userId);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Designer")]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            logger.LogInformation("Entered All Orders Page");

            IEnumerable<OrderModel> models = await orderService.GetAllAsync();
            ViewBag.HiddenOrders = $"({models.Count(m => !m.ShouldShow)} hidden)";

            ViewBag.Orders = models
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
                });

            return View(new CadInputModel());
        }

        [Authorize(Roles = "Designer")]
        [HttpPost]
        public async Task<IActionResult> BeginOrder(int cadId)
        {
            OrderModel? model = (await orderService.GetAllAsync()).FirstOrDefault(o => o.CadId == cadId);
            if (model == null)
            {
                return BadRequest();
            }

            model.Status = OrderStatus.Begun;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }

        [Authorize(Roles = "Designer")]
        [HttpPost]
        public async Task<IActionResult> FinishOrder(int cadId, CadInputModel input)
        {
            OrderModel? order = (await orderService.GetAllAsync()).FirstOrDefault(o => o.CadId == cadId);
            if (order == null)
            {
                return BadRequest();
            }

            await UploadFileAsync(input.CadFile, cadId, input.Name);

            order.Cad.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            order.Cad.CreationDate = DateTime.Now;
            order.Cad.Validated = true;

            order.Status = OrderStatus.Finished;
            await orderService.EditAsync(order);

            return RedirectToAction(nameof(All));
        }

        [Authorize(Roles = "Designer")]
        [HttpPost]
        public async Task<IActionResult> HideOrder(int cadId)
        {
            OrderModel? model = (await orderService.GetAllAsync())
                .FirstOrDefault(m => m.CadId == cadId);

            if (model == null)
            {
                return BadRequest();
            }

            model.ShouldShow = false;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }

        private async Task UploadFileAsync(IFormFile cad, int cadId, string cadName, string extension = ".stl")
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/others/cads/{cadName}{cadId}{extension}");
            using FileStream fileStream = new(filePath, FileMode.Create);
            await cad.CopyToAsync(fileStream);
        }

        public async Task<Category[]> GetCategoriesAsync()
            => (await categoryService.GetAllAsync()).ToArray();
    }
}
