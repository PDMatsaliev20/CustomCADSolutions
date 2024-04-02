using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Extensions;
using System.Drawing;
using System.Net;
using System.Text.Json;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.App.Mappings.DTOs;
using static CustomCADSolutions.App.Constants.Paths;

namespace CustomCADSolutions.App.Areas.Contributer.Controllers
{
    [Area("Contributer")]
    [Authorize(Roles = "Contributer")]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;

        public OrdersController(
            ILogger<OrdersController> logger,
            IOrderService orderService,
            ICadService cadService,
            ICategoryService categoryService,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostingEnvironment,
            HttpClient httpClient)
        {
            this.orderService = orderService;
            this.cadService = cadService;
            this.logger = logger;
            this.userManager = userManager;
            this.categoryService = categoryService;
            this.hostingEnvironment = hostingEnvironment;
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("https://localhost:7119/API/Orders/");
            mapper = new MapperConfiguration(cfg => cfg.AddProfile<OrderModelProfile>()).CreateMapper();
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

                    ViewBag.Area = "Contributer";
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

        [HttpPost]
        public async Task<IActionResult> Order(int id)
        {
            CadModel cad = await cadService.GetByIdAsync(id);
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Categories", "Home", new { area = "" });
            }

            await orderService.CreateAsync(new()
            {
                Description = $"3D Model from the gallery with id: {cad.Id}",
                OrderDate = DateTime.Now,
                Status = OrderStatus.Finished,
                ShouldShow = false,
                CadId = id,
                BuyerId = User.GetId(),
                Cad = cad,
                Buyer = await userManager.FindByIdAsync(User.GetId()),
            });

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
        public async Task<IActionResult> Delete(int id, string buyerId)
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
