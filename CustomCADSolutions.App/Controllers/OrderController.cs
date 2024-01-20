﻿using CustomCADSolutions.App.Models;
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

            UserModel? buyer = service.GetAllUsers().FirstOrDefault(u => u.Username == u.Username);

            OrderModel model = new()
            {
                Buyer = buyer ?? new UserModel { Username = "John Doe" },
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
