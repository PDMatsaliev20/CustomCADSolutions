using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomCADSolutions.App.Areas.Contributer.Controllers
{
    [Area("Contributer")]
    [Authorize(Roles = "Contributer")]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ICadService cadService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public OrderController(
            ILogger<OrderController> logger,
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

                if (model.Creator == null || !model.CreationDate.HasValue)
                {
                    return BadRequest("Model not created yet");
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
                SpinAxis = model.SpinAxis,
                SpinFactor = model.SpinFactor
            };
            return View(view);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation("Entered Orders Page");

            IEnumerable<OrderViewModel> orders = (await orderService.GetAllAsync())
                .Where(o => o.BuyerId == GetUserId())
                .OrderBy(o => (int)o.Status).ThenBy(o => o.OrderDate)
                .Select(m => new OrderViewModel
                {
                    BuyerId = m.BuyerId,
                    BuyerName = m.Buyer.UserName,
                    CadId = m.CadId,
                    Category = m.Cad.Category.Name,
                    BgCategory = m.Cad.Category.BgName,
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
                logger.LogError("Invalid Order");
                return RedirectToAction("Categories", "Home", new { area = "" });
            }

            await orderService.CreateAsync(new()
            {
                Description = $"3D Model from the gallery with id: {cad.Id}",
                OrderDate = DateTime.Now,
                Status = OrderStatus.Finished,
                ShouldShow = false,
                CadId = id,
                BuyerId = GetUserId(),
                Cad = cad,
                Buyer = await userManager.FindByIdAsync(GetUserId()),
            });

            logger.LogInformation("Ordered 3d model");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            logger.LogInformation("Entered Order Page");

            OrderInputModel input = new() { Categories = await GetCategoriesAsync() };
            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order");
                input.Categories = await GetCategoriesAsync();
                return View(input);
            }

            OrderModel model = new()
            {
                Description = input.Description,
                OrderDate = DateTime.Now,
                Status = input.Status,
                ShouldShow = true,
                Buyer = await userManager.FindByIdAsync(GetUserId()),
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
            logger.LogInformation("Entered Edit Order Page");

            try
            {
                OrderModel model = await orderService.GetByIdAsync(cadId, GetUserId());

                if (model.Status != OrderStatus.Pending)
                {
                    return RedirectToAction(nameof(Index));
                }

                OrderInputModel input = new()
                {
                    Categories = await GetCategoriesAsync(),
                    Status = model.Status,
                    CadId = model.CadId,
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

            if (GetUserId() != buyerId)
            {
                return Unauthorized();
            }

            OrderModel order = await orderService.GetByIdAsync(cadId, buyerId);
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
            if (GetUserId() != buyerId)
            {
                return Unauthorized();
            }

            await orderService.DeleteAsync(cadId, buyerId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<FileResult> DownloadCad(int id)
        {
            string name = (await cadService.GetByIdAsync(id)).Name;
            string filePath = GetCadPath(name, id);
            return PhysicalFile(filePath, "application/sla", name + ".stl");
        }

        private static async Task UploadFileAsync(IFormFile cad, int cadId, string cadName, string extension = ".stl")
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/others/cads/{cadName}{cadId}{extension}");
            using FileStream fileStream = new(filePath, FileMode.Create);
            await cad.CopyToAsync(fileStream);
        }

        private string GetCadPath(string cadName, int cadId)
            => Path.Combine(hostingEnvironment.WebRootPath, "others", "cads", $"{cadName}{cadId}.stl");

        public async Task<Category[]> GetCategoriesAsync()
            => (await categoryService.GetAllAsync()).ToArray();

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
