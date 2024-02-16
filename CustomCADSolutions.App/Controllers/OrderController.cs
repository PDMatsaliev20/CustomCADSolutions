using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomCADSolutions.App.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;

        public OrderController(
            ILogger<OrderController> logger,
            IOrderService orderService,
            UserManager<IdentityUser> userManager)
        {
            this.orderService = orderService;
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation("Entered Orders Page");

            string username = User.FindFirstValue(ClaimTypes.Name);
            IdentityUser user = await userManager.FindByNameAsync(username);
            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            IEnumerable<OrderViewModel> orders = (await orderService.GetAllAsync())
                .Where(o => o.Buyer == user)
                .OrderBy(o => (int)o.Status)
                    .ThenBy(o => o.OrderDate)
                .Select(m => new OrderViewModel
                {
                    BuyerId = m.BuyerId,
                    BuyerName = m.Buyer.UserName,
                    CadId = m.CadId,
                    Category = m.Cad.Category.ToString(),
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

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser user = await userManager.FindByIdAsync(userId);
            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            return View(new OrderInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order");
                return View(input);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser user = await userManager.FindByIdAsync(userId);
            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            OrderModel model = new()
            {
                Description = input.Description,
                OrderDate = DateTime.Now,
                Status = input.Status,
                Buyer = user,
                Cad = new CadModel()
                {
                    Name = input.Name,
                    Category = (Category)input.Category,
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
            IdentityUser user = await userManager.FindByIdAsync(userId);
            OrderModel model = await orderService.GetByIdAsync(cadId, userId);
            OrderInputModel input = new();
            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                input = new()
                {
                    Status = model.Status,
                    CadId = model.CadId,
                    Name = model.Cad.Name,
                    Category = (int)model.Cad.Category,
                    Description = model.Description,
                    OrderDate = model.OrderDate,
                };
            }
            else
            {
                input = new()
                {
                    Status = model.Status,
                    CadId = model.CadId,
                    Name = model.Cad.Name,
                    Category = (int)model.Cad.Category,
                    Description = model.Description,
                    OrderDate = model.OrderDate,
                };
            }

            return View(input);
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
            IdentityUser user = await userManager.FindByIdAsync(userId);
            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            OrderModel order = await orderService.GetByIdAsync(cadId, userId);
            order.Cad.Name = input.Name;
            order.Description = input.Description;
            order.Cad.Category = (Category)input.Category;
            await orderService.EditAsync(order);

            logger.LogInformation("Edited Order");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int cadId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser user = await userManager.FindByIdAsync(userId);

            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            await orderService.DeleteAsync(cadId, userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int cadId, string status)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser user = await userManager.FindByIdAsync(userId);

            if (!await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            OrderModel? model = (await orderService.GetAllAsync()).FirstOrDefault(o => o.CadId == cadId);
            if (model == null)
            {
                return BadRequest();
            }

            OrderStatus orderStatus = default;
            if (!Enum.TryParse<OrderStatus>(status, out orderStatus))
            {
                return BadRequest();
            }

            model.Status = orderStatus;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            logger.LogInformation("Entered All Orders Page");

            string username = User.FindFirstValue(ClaimTypes.Name);
            IdentityUser user = await userManager.FindByNameAsync(username);

            IEnumerable<OrderViewModel> orders = (await orderService.GetAllAsync())
                .OrderBy(o => o.OrderDate)
                .Select(m => new OrderViewModel
                {
                    BuyerId = m.BuyerId,
                    BuyerName = m.Buyer.UserName,
                    CadId = m.CadId,
                    Category = m.Cad.Category.ToString(),
                    Name = m.Cad.Name,
                    Description = m.Description,
                    Status = m.Status.ToString(),
                    OrderDate = m.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"),
                });

            return View(orders);
        }
    }
}
