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
            OrderInputModel input = new();
            ViewData["Categories"] = typeof(Category).GetEnumValues();
            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Index(OrderInputModel input)
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
            OrderInputModel model = new()
            {
                Name = order.Cad.Name,
                Category = order.Cad.Category,
                Description = order.Description,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sent(OrderModel model)
        {
            await service.EditAsync(model);

            OrderInputModel view = new()
            {
                Name = model.Cad.Name,
                Description = model.Description,
            };

            return RedirectToAction("Sent", view);
        }
    }
}
