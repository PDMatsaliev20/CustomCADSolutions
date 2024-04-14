﻿using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using CustomCADSolutions.App.Extensions;
using Microsoft.Extensions.Options;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.Core.Mappings.DTOs;
using CustomCADSolutions.Core.Mappings.CadDTOs;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.App.Models.Cads.View;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Humanizer;
using CustomCADSolutions.Core.Services;

namespace CustomCADSolutions.App.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class OrdersController : Controller
    {
        // Services
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly ICadService cadService;

        // Addons
        private readonly StripeSettings stripeSettings;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public OrdersController(
            IOrderService orderService,
            ICadService cadService,
            ICategoryService categoryService,
            IOptions<StripeSettings> stripeSettings,
            ILogger<OrdersController> logger)
        {
            this.orderService = orderService;
            this.categoryService = categoryService;
            this.cadService = cadService;

            this.logger = logger;
            this.stripeSettings = stripeSettings.Value;
            MapperConfiguration config = new(cfg =>
            {
                cfg.AddProfile<OrderAppProfile>();
                cfg.AddProfile<CadDTOProfile>();
            });
            this.mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<OrderModel> orders = await orderService.GetAllAsync();
            var buyerOrders = orders.Where(o => o.Buyer.UserName == User.Identity!.Name);

            var views = mapper.Map<OrderViewModel[]>(buyerOrders);
            return View(views);
        }

        [HttpGet]
        public async Task<IActionResult> Order(int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            CadViewModel view = mapper.Map<CadViewModel>(model);

            ViewBag.StripeKey = stripeSettings.TestPublishableKey;
            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> Order(int id, string stripeToken)
        {
            CadModel cadModel = await cadService.GetByIdAsync(id);

            bool isPaymentSuccessful = stripeSettings.ProcessPayment(stripeToken);
            if (!isPaymentSuccessful)
            {
                TempData["ErrorMessage"] = "Payment failed. Please try again.";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            OrderModel model = new()
            {
                Name = cadModel.Name,
                Description = $"3D Model from the gallery with id: {id}",
                Status = OrderStatus.Finished,
                CategoryId = cadModel.CategoryId,
                OrderDate = DateTime.Now,
                CadId = id,
                BuyerId = User.GetId(),
            };
            await orderService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new OrderInputModel()
            {
                Categories = await categoryService.GetAllAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Categories = await categoryService.GetAllAsync();
                return View(input);
            }

            OrderModel model = mapper.Map<OrderModel>(input);
            model.BuyerId = User.GetId();
            model.OrderDate = DateTime.Now;

            await orderService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            OrderModel model = await orderService.GetByIdAsync(id);
            if (model.Status != OrderStatus.Pending)
            {
                return RedirectToAction(nameof(Index));
            }

            OrderInputModel input = mapper.Map<OrderInputModel>(model);
            input.Categories = await categoryService.GetAllAsync();

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderInputModel input)
        {
            if (input.Status != OrderStatus.Pending)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                input.Categories = await categoryService.GetAllAsync();
                return View(input);
            }

            OrderModel model = mapper.Map<OrderModel>(input);
            await orderService.EditAsync(id, model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await orderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
