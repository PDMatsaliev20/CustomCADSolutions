using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;

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
                return View(input);
            }

            OrderModel model = new()
            {
                Buyer = new UserModel { Name = "John Doe" },
                Description = input.Description,
                OrderDate = DateTime.Now,
                Cad = new CadModel()
                {
                    Category = Enum.Parse<Category>(input.CadCategory),
                    Name = input.CadName,
                }
            };
            await service.CreateAsync(model);

            OrderViewModel view = new()
            {
                Description = model.Description,
                OrderDate = model.OrderDate.ToString("dd/mm/yyyy hh:MM:ss")
            };

            return RedirectToAction("Sent", view);
        }

        [HttpGet]
        public IActionResult Sent(OrderViewModel view)
        {
            if (!ModelState.IsValid)
            {
                return View(view);
            }
            return View(view);
        }
    }
}
