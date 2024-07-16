using AutoMapper;
using CustomCADs.App.Extensions;
using CustomCADs.App.Mappings;
using CustomCADs.App.Models.Cads.View;
using CustomCADs.App.Models.Orders;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.App.Controllers
{
    [Authorize(Roles = $"{Client},{Contributor}")]
    public class OrdersController(
        IOrderService orderService,
        ICadService cadService,
        ICategoryService categoryService,
        IOptions<StripeInfo> stripeOptions,
        ILogger<OrdersController> logger) : Controller
    {
        private readonly StripeInfo stripe = stripeOptions.Value;
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OrderAppProfile>();
                cfg.AddProfile<CadAppProfile>();
            }).CreateMapper();

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
            ViewBag.StripeKey = stripe.TestPublishableKey;
            CadModel model = await cadService.GetByIdAsync(id);
            return View(mapper.Map<CadViewModel>(model));
        }

        [HttpPost]
        public async Task<IActionResult> Order(int id, string stripeToken)
        {
            CadModel cad = await cadService.GetByIdAsync(id);
            
            bool isPaymentSuccessful = stripe.ProcessPayment(stripeToken, cad.Name, cad.Price);
            if (!isPaymentSuccessful)
            {
                TempData["ErrorMessage"] = "Payment failed. Please try again.";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            OrderModel order = new()
            {
                Name = cad.Name,
                Description = $"3D Model from the gallery with id: {id}",
                Status = OrderStatus.Finished,
                CategoryId = cad.CategoryId,
                OrderDate = DateTime.Now,
                CadId = id,
                BuyerId = User.GetId(),
            };
            await orderService.CreateAsync(order);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new OrderAddModel()
            {
                Categories = await categoryService.GetAllAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderAddModel input)
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

            var input = mapper.Map<OrderEditModel>(model);
            input.Categories = await categoryService.GetAllAsync();

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderEditModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Categories = await categoryService.GetAllAsync();
                return View(input);
            }

            OrderModel model = mapper.Map<OrderModel>(input);
            
            if (model.Status != OrderStatus.Pending)
            {
                return RedirectToAction(nameof(Index));
            }

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
