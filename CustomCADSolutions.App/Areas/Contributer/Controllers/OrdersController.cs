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

        private const string AllPath = "All";
        private const string ByIdPath = "Single?cadId={0}&buyerId={1}";
        private const string CreatePath = "Create";
        private const string EditPath = "Edit";
        private const string DeletePath = "Delete?cadId={0}&buyerId={1}";

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
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await httpClient.GetAsync($"Single?cadId={id}&buyerId={User.GetId()}");

            if (response.IsSuccessStatusCode)
            {
                Stream body = await response.Content.ReadAsStreamAsync();

                CadViewModel? result = await JsonSerializer
                    .DeserializeAsync<CadViewModel>(body);

                return result == null ? BadRequest() : View(result);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            else return BadRequest();
            /*
            CadModel model;
            try
            {
                model = await cadService.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            if (model.Bytes == null || model.Creator == null || !model.CreationDate.HasValue)
            {
                return BadRequest("Model not created yet");
            }

            CadViewModel view = new()
            {
                Id = model.Id,
                Cad = model.Bytes,
                Name = model.Name,
                Category = model.Category.Name,
                CreatorName = model.Creator.UserName,
                CreationDate = model.CreationDate.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                Coords = model.Coords,
                SpinAxis = model.SpinAxis
            };
            return View(view);
            */
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            OrderViewModel[] views;
            bool success = httpClient.TryGet(AllPath, out views!);
            return success ? 
                View(views.Where(v => v.BuyerId == User.GetId()).ToArray()) :
                BadRequest();
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

            OrderModel order;
            bool success = httpClient.TryPost(CreatePath, input, out order!);
            
            return success ? RedirectToAction(nameof(Details),
                    new { cadId = order.CadId, buyerId = order.BuyerId }) 
                : BadRequest();
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
                    CadId = model.CadId,
                    BuyerId = model.BuyerId,
                    Name = model.Cad.Name,
                    Description = model.Description,
                    CategoryId = model.Cad.CategoryId,
                    Categories = await categoryService.GetAllAsync(),
                };

                return View(input);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError($"Invalid Order {string.Join(", ", ModelState.GetErrors())}");
                input.Categories = await categoryService.GetAllAsync();
                return View(input);
            }

            OrderModel order = new();
            try
            {
                order = await orderService.GetByIdAsync(input.CadId, input.BuyerId);
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
        public async Task<IActionResult> ChangeColor(int id, string color)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            model.Color = ColorTranslator.FromHtml(color);
            await cadService.EditAsync(model);

            return RedirectToAction(nameof(Index), "Cads");
        }
    }
}
