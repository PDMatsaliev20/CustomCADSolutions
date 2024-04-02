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
using System.Drawing;
using CustomCADSolutions.App.Models;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using System.Text.Json;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.App.Mappings.DTOs;
using CustomCADSolutions.App.Mappings.CadDTOs;

namespace CustomCADSolutions.App.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ICadService cadService;
        private readonly ILogger logger;
        private readonly ICategoryService categoryService;
        private readonly StripeSettings stripeSettings;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public OrdersController(
            ILogger<OrdersController> logger,
            IOrderService orderService,
            ICadService cadService,
            ICategoryService categoryService,
            IOptions<StripeSettings> stripeSettings,
            HttpClient httpClient)
        {
            // Services
            this.orderService = orderService;
            this.cadService = cadService;
            this.categoryService = categoryService;
            
            // Helpers
            this.logger = logger;
            this.httpClient = httpClient;
            
            // Addons
            this.stripeSettings = stripeSettings.Value;
            MapperConfiguration config = new(cfg => cfg.AddProfile<OrderDTOProfile>());
            this.mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var dtos = await httpClient.TryGetFromJsonAsync<IEnumerable<OrderExportDTO>>(OrdersPath);
                if (dtos != null)
                {
                    dtos = dtos.Where(v => v.BuyerName == User.Identity!.Name);

                    ViewBag.Area = "Client";
                    return View(mapper.Map<OrderViewModel[]>(dtos));
                }
                else throw new JsonException("Json parsing error");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!await orderService.ExistsByIdAsync(id, User.GetId()))
            {
                logger.LogWarning($"User {User.Identity!.Name} doesn't have an order with id: {id}");
                return Unauthorized();
            }

            string path = string.Format(OrderByIdPath, id, User.GetId());
            OrderImportDTO? dto = await httpClient.TryGetFromJsonAsync<OrderImportDTO>(path);

            if (dto != null)
            {
                return View(mapper.Map<OrderViewModel>(dto));
            }
            else return BadRequest("Json parsing error");
        }

        [HttpGet]
        public async Task<IActionResult> Order(int id)
        {
            if (!await cadService.ExistsByIdAsync(id) || id == 1)
            {
                return BadRequest("Model not found");
            }

            string path = string.Format(CadByIdPath, id);
            CadExportDTO? dto = await httpClient.TryGetFromJsonAsync<CadExportDTO>(path);

            if (dto != null)
            {
                CadViewModel view = mapper.Map<CadViewModel>(dto);
                ViewBag.StripeKey = stripeSettings.TestPublishableKey;
                return View(view);
            }
            else return BadRequest("Json parsing error");
        }

        [HttpPost]
        public async Task<IActionResult> Order(int id, string stripeToken)
        {
            if (await orderService.ExistsByIdAsync(id, User.GetId()))
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            bool succeeded = stripeSettings.ProcessPayment(stripeToken);
            if (!succeeded)
            {
                TempData["ErrorMessage"] = "Payment failed. Please try again.";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            OrderImportDTO dto = new()
            {
                CadId = id,
                BuyerId = User.GetId(),
                Description = $"3D Model from the gallery with id: {id}",
                Status = OrderStatus.Finished.ToString(),
            };
            await httpClient.PostAsJsonAsync(CreateOrderPath, dto);

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

            try
            {
                input.BuyerId = User.GetId();
                var dto = mapper.Map<OrderImportDTO>(input);
                var response = await httpClient.PostAsJsonAsync(CreateOrderPath, dto);
                response.EnsureSuccessStatusCode();
                
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
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
                    CadId = model.CadId,
                    BuyerId = model.BuyerId,
                    Name = model.Cad.Name,
                    CategoryId = model.Cad.CategoryId,
                    Description = model.Description,
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
                logger.LogError($"Invalid Order - {string.Join(", ", ModelState.GetErrors())}");
                input.Categories = await categoryService.GetAllAsync();
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
        public async Task<IActionResult> Delete(string buyerId, int id)
        {
            string path = string.Format(DeleteOrderPath, buyerId, id);
            var response = await httpClient.DeleteAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else return StatusCode((int)response.StatusCode);
        }
    }
}
