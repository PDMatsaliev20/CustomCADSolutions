using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security;

namespace CustomCADSolutions.App.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService service;

        public OrderController(IOrderService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Index()
            {
            OrderViewModel input = new();
            ViewData["Categories"] = typeof(Category).GetEnumValues();
            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Index(OrderViewModel input)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = typeof(Category).GetEnumValues();
                return View();
            }

            UserModel? buyer = service.GetAllUsers().FirstOrDefault(u => u.Username == u.Username);

            OrderModel model = new()
            {
                Buyer = buyer ?? new UserModel { Username = "John Doe" },
                Description = input.Description,
                OrderDate = DateTime.Now,
                Cad = new CadModel()
                {
                    Category = input.Category,
                    Name = input.Name,
                }
            };

            int id = await service.CreateAsync(model);

            return RedirectToAction("Sent", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Sent(int id)
        {
            ViewData["Categories"] = typeof(Category).GetEnumValues();
            OrderModel order = await service.GetByIdAsync(id);
            OrderViewModel model = new()
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
        public async Task<IActionResult> Sent(OrderViewModel model)
        {
            IEnumerable<OrderModel> orders = await service.GetAllAsync();
            OrderModel order = orders.First(o => model.Id == o.Id);

            order.Cad.Name = model.Name;
            order.Description = model.Description;
            order.Cad.Category = model.Category;
            await service.EditAsync(order);

            return RedirectToAction("Sent", order.Id);
        }

        private void SendToBorko()
        {
        }
    }
}
