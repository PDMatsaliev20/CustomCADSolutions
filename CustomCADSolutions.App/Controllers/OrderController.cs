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
            logger.LogInformation("Entered All Orders Page");

            string username = User.FindFirstValue(ClaimTypes.Name);
            IEnumerable<OrderViewModel> orders = (await orderService.GetAllAsync())
                .Select(o => new OrderViewModel
                {
                    BuyerId = o.BuyerId,
                    BuyerName = o.Buyer.UserName,
                    CadId = o.CadId,
                    Category = o.Cad.Category.ToString(),
                    Name = o.Cad.Name,
                    Description = o.Description,
                    OrderDate = o.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"),
                });

            IdentityUser user = await userManager.FindByNameAsync(username);
            if (!await userManager.IsInRoleAsync(user, "Administrator"))
            {
                orders = orders.Where(o => o.BuyerName == username);
            }

            return View(orders);
        }

        [Authorize(Roles = "Contributer")]
        [HttpGet]
        public IActionResult Add()
        {
            logger.LogInformation("Entered Order Page");
            return View(new OrderInputModel());
        }

        [Authorize(Roles = "Contributer")]
        [HttpPost]
        public async Task<IActionResult> Add(OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order");
                return View(input);
            }

            OrderModel model = new()
            {
                Description = input.Description,
                OrderDate = DateTime.Now,
                Buyer = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)),
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

        [Authorize(Roles = "Contributer")]
        [HttpGet]
        public async Task<IActionResult> Edit(int cadId)
        {
            logger.LogInformation("Entered Edit Order Page");

            string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            OrderModel order = await orderService.GetByIdAsync(cadId, buyerId);

            OrderInputModel model = new()
            {
                CadId = order.CadId,
                Name = order.Cad.Name,
                Category = (int)order.Cad.Category,
                Description = order.Description,
                OrderDate = order.OrderDate,
            };

            return View(model);
        }

        [Authorize(Roles = "Contributer")]
        [HttpPost]
        public async Task<IActionResult> Edit(OrderInputModel input, int cadId)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order");
                return View(input);
            }

            string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            OrderModel order = await orderService.GetByIdAsync(cadId, buyerId);

            order.Cad.Name = input.Name;
            order.Description = input.Description;
            order.Cad.Category = (Category)input.Category;
            await orderService.EditAsync(order);

            logger.LogInformation("Edited Order");
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Contributer")]
        [HttpPost]
        public async Task<IActionResult> Delete(int cadId)
        {
            string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await orderService.DeleteAsync(cadId, buyerId);

            return RedirectToAction(nameof(Index));
        }
    }
}
