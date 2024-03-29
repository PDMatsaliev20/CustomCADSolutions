using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using CustomCADSolutions.App.Extensions;
using Microsoft.Extensions.Options;
using Stripe;
using System.Drawing;

namespace CustomCADSolutions.App.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly StripeSettings stripeSettings;

        public OrdersController(
            ILogger<OrdersController> logger,
            IOrderService orderService,
            ICadService cadService,
            ICategoryService categoryService,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostingEnvironment,
            IOptions<StripeSettings> stripeSettings)
        {
            this.orderService = orderService;
            this.cadService = cadService;
            this.logger = logger;
            this.userManager = userManager;
            this.categoryService = categoryService;
            this.hostingEnvironment = hostingEnvironment;
            this.stripeSettings = stripeSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            CadModel model;
            try
            {
                model = await cadService.GetByIdAsync(id);

                if (model.Creator == null || !model.CreationDate.HasValue)
                {
                    throw new Exception("Model not created yet");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            CadViewModel view = new()
            {
                Id = model.Id,
                Name = model.Name,
                Category = model.Category.Name,
                CreatorName = model.Creator.UserName,
                CreationDate = model.CreationDate.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                Coords = model.Coords,
                SpinAxis = model.SpinAxis
            };
            return View(view);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation("Entered Orders Page");

            IEnumerable<OrderViewModel> orders = (await orderService.GetAllAsync())
                .Where(o => o.BuyerId == User.GetId())
                .OrderBy(o => (int)o.Status).ThenBy(o => o.OrderDate)
                .Select(m => new OrderViewModel
                {
                    BuyerId = m.BuyerId,
                    BuyerName = m.Buyer.UserName,
                    CadId = m.CadId,
                    Category = m.Cad.Category.Name,
                    Name = m.Cad.Name,
                    Description = m.Description,
                    Status = m.Status.ToString(),
                    OrderDate = m.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"),
                });

            ViewBag.Orders = orders;
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Order(int id)
        {
            if (!await cadService.ExistsByIdAsync(id))
            {
                return BadRequest("Model not found");
            }

            CadModel model = await cadService.GetByIdAsync(id);
            CadViewModel view = new()
            {
                Id = model.Id,
                Name = model.Name,
                Category = model.Category.Name,
                CreatorName = model.Creator?.UserName!,
                CreationDate = model.CreationDate?.ToString("dd/MM/yyyy HH:mm:ss"),
                Coords = model.Coords,
                SpinAxis = model.SpinAxis,
                RGB = (model.Color.R, model.Color.B, model.Color.G) 
            };
            ViewBag.StripeKey = stripeSettings.PublishableKey;
            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> Order(CadViewModel model, string stripeToken)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError($"Erros: {string.Join(", ", ModelState.GetErrors())}");
                //return RedirectToAction("Categories", "Home", new { area = "" });
            }

            bool succeeded = stripeSettings.ProcessPayment(stripeToken);
            if (!succeeded)
            {
                TempData["ErrorMessage"] = "Payment failed. Please try again.";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            await orderService.CreateAsync(new()
            {
                Description = $"3D Model from the gallery with id: {model.Id}",
                OrderDate = DateTime.Now,
                Status = OrderStatus.Finished,
                ShouldShow = false,
                CadId = model.Id,
                BuyerId = User.GetId(),
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            OrderInputModel input = new() { Categories = await categoryService.GetAllAsync() };
            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order: {0}", string.Join(", ", ModelState.GetErrors()));
                input.Categories = await categoryService.GetAllAsync();
                return View(input);
            }

            OrderModel model = new()
            {
                Description = input.Description,
                OrderDate = DateTime.Now,
                Status = input.Status,
                ShouldShow = true,
                Buyer = await userManager.FindByIdAsync(User.GetId()),
                Cad = new CadModel()
                {
                    Name = input.Name,
                    CategoryId = input.CategoryId,
                }
            };
            await orderService.CreateAsync(model);

            logger.LogInformation("Ordered 3d model");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int cadId)
        {
            try
            {
                OrderModel model = await orderService.GetByIdAsync(cadId, User.GetId());

                if (model.Status != OrderStatus.Pending)
                {
                    return RedirectToAction(nameof(Index));
                }

                OrderInputModel input = new()
                {
                    Categories = await categoryService.GetAllAsync(),
                    Status = model.Status,
                    CadId = model.CadId,
                    BuyerId = model.BuyerId,
                    Name = model.Cad.Name,
                    CategoryId = model.Cad.CategoryId,
                    Description = model.Description,
                    OrderDate = model.OrderDate,
                };

                return View(input);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int cadId, string buyerId, OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order");
                return View(input);
            }

            OrderModel order = new();
            try
            {
                order = await orderService.GetByIdAsync(cadId, buyerId);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }

            if (order.Status != OrderStatus.Pending)
            {
                return RedirectToAction(nameof(Index));
            }

            order.Cad.Name = input.Name;
            order.Description = input.Description;
            order.Cad.CategoryId = input.CategoryId;
            await orderService.EditAsync(order);

            logger.LogInformation("Edited Order");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int cadId, string buyerId)
        {
            try
            {
                await orderService.DeleteAsync(cadId, buyerId);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Hide(CadInputModel input)
        {
            OrderModel? model = await orderService.GetByIdAsync(input.Id, input.BuyerId!);

            if (model == null)
            {
                return BadRequest();
            }

            model.ShouldShow = false;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeColor(int id, string color)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            model.Color = ColorTranslator.FromHtml(color);
            await cadService.EditAsync(model);

            return RedirectToAction(nameof(Index), "Cads");
        }
    }
}
