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
        private const string password = "jaradfrv";

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
                    Category = Enum.Parse<Category>(input.Category),
                    Name = input.Name,
                }
            };
            await service.CreateAsync(model);

            //SendEmail(model);

            OrderViewModel view = new()
            {
                Description = model.Description,
            };

            return RedirectToAction("Sent", view);
        }

        [HttpGet]
        public IActionResult Sent(OrderViewModel view)
        {
            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string description)
        {
            await service.EditAsync(default!);
            return RedirectToAction("Sent", new OrderViewModel() { Id = id, Description = description });
        }
    }
}
