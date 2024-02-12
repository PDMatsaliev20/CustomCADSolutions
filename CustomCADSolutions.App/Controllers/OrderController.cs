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
        private readonly SignInManager<IdentityUser> signInManager;

        public OrderController(
            ILogger<OrderController> logger, 
            IOrderService orderService, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.orderService = orderService;
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation("Entered All Orders Page");

            string username = User.FindFirstValue(ClaimTypes.Name);
            IEnumerable<OrderViewModel> orders = (await orderService.GetAllAsync())
                .Where(o => o.Buyer.UserName == username)
                .Select(o => new OrderViewModel
                {
                    CadId = o.CadId,
                    BuyerId = o.BuyerId,
                    Category = o.Cad.Category.ToString(),
                    Name = o.Cad.Name,
                    Description = o.Description,
                    OrderDate = o.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"),
                });

            ViewBag.UserName = username;
            return View(orders);
        }

        [HttpGet]
        public IActionResult Add()
        {
            logger.LogInformation("Entered Order Page");
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

        [HttpPost]
        public async Task<IActionResult> Delete(int cadId)
        {
            string buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await orderService.DeleteAsync(cadId, buyerId);

            return RedirectToAction(nameof(Index));
        }
    }
}
