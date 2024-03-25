using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.AppWithIdentity.Data.Migrations;
using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Attributes;

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

        public OrdersController(
            ILogger<OrdersController> logger,
            IOrderService orderService,
            ICadService cadService,
            ICategoryService categoryService,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostingEnvironment)
        {
            this.orderService = orderService;
            this.cadService = cadService;
            this.logger = logger;
            this.userManager = userManager;
            this.categoryService = categoryService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            CadModel model;
            try
            {
                model = await cadService.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            if (model.Creator == null || !model.CreationDate.HasValue)
            {
                return BadRequest("Model not created yet");
            }

            CadViewModel view = new()
            {
                Id = model.Id,
                Name = model.Name,
                Category = model.Category.Name,
                CreatorName = model.Creator.UserName,
                CreationDate = model.CreationDate.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                Coords = model.Coords,
                SpinAxis = model.SpinAxis,
                SpinFactor = model.SpinFactor
            };
            return View(view);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
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

            return View(orders);
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
            OrderInputModel input = new() { Categories = await categoryService.GetAllAsync() };
            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
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

        [HttpGet]
        public async Task<FileResult> DownloadCad(int id)
        {
            string name = (await cadService.GetByIdAsync(id)).Name;
            string filePath = hostingEnvironment.GetCadPath(name, id);
            return PhysicalFile(filePath, "application/sla", name + ".stl");
        }
    }
}
