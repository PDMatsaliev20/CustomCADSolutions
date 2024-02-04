using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ILogger logger;

        public OrderController(IOrderService service, ILogger<OrderController> logger)
        {
            this.orderService = service;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            logger.LogInformation("Entered Order Page");
            return View(new OrderInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(OrderInputModel input)
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
                Buyer = orderService.GetAllUsers().First(),
                Cad = new CadModel()
                {
                    Name = input.Name,
                    Category = (Category)input.Category,
                }
            };
            int id = await orderService.CreateAsync(model);

            logger.LogInformation("Ordered 3d model");
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            logger.LogInformation("Entered Edit Order Page");

            OrderModel order = await orderService.GetByIdAsync(id);
            OrderInputModel model = new()
            {
                Id = id,
                Name = order.Cad.Name,
                Category = (int)order.Cad.Category,
                Description = order.Description,
                OrderDate = order.OrderDate,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderInputModel input, int id)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order");
                return View(input);
            }

            //Model's ID is saved seperately from the model itself
            OrderModel order = (await orderService.GetAllAsync()).First(o => id == o.Id);

            order.Cad.Name = input.Name;
            order.Description = input.Description;
            order.Cad.Category = (Category)input.Category;
            await orderService.EditAsync(order);

            logger.LogInformation("Edited Order");
            return RedirectToAction(nameof(All), routeValues: new { order.Buyer.Id });
        }

        [HttpGet]
        public IActionResult All(int id)
        {
            logger.LogInformation("Entered All Orders Page");
            UserModel user = orderService.GetAllUsers().First(u => u.Id == id);

            IEnumerable<OrderViewModel> orders = user.Orders
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    Category = o.Cad.Category.ToString(),
                    Name = o.Cad.Name,
                    Description = o.Description,
                    OrderDate = o.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"),
                });

            ViewBag.UserName = user.Username;
            return View(orders);
        }

        private void SendToBorko()
        {
        }
    }
}
