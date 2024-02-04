using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using CustomCADSolutions.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService service;
        private readonly ILogger logger;

        public OrderController(IOrderService service, ILogger<OrderController> logger)
        {
            this.service = service;
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
                return Error();
            }

            OrderModel model = new()
            {
                Buyer = service.GetAllUsers().First(),
                Description = input.Description,
                OrderDate = DateTime.Now,
                Cad = new CadModel()
                {
                    Category = input.Category,
                    Name = input.Name,
                }
            };

            int id = await service.CreateAsync(model);

            logger.LogInformation("Ordered 3d model");
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            OrderModel order = await service.GetByIdAsync(id);
            OrderInputModel model = new()
            {
                Id = id,
                Name = order.Cad.Name,
                Category = order.Cad.Category,
                Description = order.Description,
                OrderDate = order.OrderDate,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderInputModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }

            //Model's ID is saved seperately from the model itself
            OrderModel order = (await service.GetAllAsync()).First(o => id == o.Id);

            order.Cad.Name = model.Name;
            order.Description = model.Description;
            order.Cad.Category = model.Category;
            await service.EditAsync(order);

            return RedirectToAction(nameof(CadController.Index), "Cad", routeValues: new { category = order.Cad.Category.ToString() });
        }

        private void SendToBorko()
        {
        }

        private IActionResult Error() => RedirectToAction("Error", "Home");
    }
}
