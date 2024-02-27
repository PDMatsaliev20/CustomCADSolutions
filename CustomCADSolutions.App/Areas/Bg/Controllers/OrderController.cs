using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomCADSolutions.App.Areas.Bg.Controllers
{
    [Area("Bg")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly Dictionary<int, string> bgCategories = new()
        {
            [1] = "Животни",
            [2] = "Герои",
            [3] = "Електроники",
            [4] = "Мода",
            [5] = "Мебели",
            [6] = "Природа",
            [7] = "Наука",
            [8] = "Спорт",
            [9] = "Играчки",
            [10] = "Автомобили",
            [11] = "Други",
        };
        private readonly string[] bgStatuses =
        {
            "В очакване",
            "Започната",
            "Завършена"
        };


        public OrderController(
            ILogger<OrderController> logger,
            IOrderService orderService,
            UserManager<IdentityUser> userManager)
        {
            this.orderService = orderService;
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation("Entered Orders Page");

            string username = User.FindFirstValue(ClaimTypes.Name);
            IdentityUser user = await userManager.FindByNameAsync(username);
            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            IEnumerable<OrderViewModel> orders = (await orderService.GetAllAsync())
                .Where(o => o.Buyer == user)
                .OrderBy(o => (int)o.Status)
                    .ThenBy(o => o.OrderDate)
                .Select(m => new OrderViewModel
                {
                    BuyerId = m.BuyerId,
                    BuyerName = m.Buyer.UserName,
                    CadId = m.CadId,
                    Category = bgCategories[m.Cad.Category.Id],
                    Name = m.Cad.Name,
                    Description = m.Description,
                    Status = bgStatuses[(int)m.Status],
                    OrderDate = m.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"),
                });

            return View(orders);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            logger.LogInformation("Entered All Orders Page");

            string username = User.FindFirstValue(ClaimTypes.Name);
            IdentityUser user = await userManager.FindByNameAsync(username);

            IEnumerable<OrderModel> models = await orderService.GetAllAsync();
            ViewBag.HiddenOrders = $"({models.Count(m => !m.ShouldShow)} скрити)";

            IEnumerable<OrderViewModel> views = models
                .Where(m => m.ShouldShow)
                .OrderBy(o => o.OrderDate)
                .Select(m => new OrderViewModel
                {
                    BuyerId = m.BuyerId,
                    BuyerName = m.Buyer.UserName,
                    CadId = m.CadId,
                    Category = bgCategories[m.Cad.CategoryId],
                    Name = m.Cad.Name,
                    Description = m.Description,
                    Status = m.Status.ToString(),
                    OrderDate = m.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"),
                });

            ViewBag.BgStatuses = bgStatuses;
            return View(views);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int cadId, string status)
        {
            OrderModel? model = (await orderService.GetAllAsync())
                .FirstOrDefault(o => o.CadId == cadId);
            
            if (model == null)
            {
                return BadRequest();
            }

            if (!Enum.TryParse(status, out OrderStatus orderStatus))
            {
                return BadRequest();
            }

            model.Status = orderStatus;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Hide(int cadId)
        {
            OrderModel? model = (await orderService.GetAllAsync())
                .FirstOrDefault(m => m.CadId == cadId);

            if (model == null)
            {
                return BadRequest();
            }

            model.ShouldShow = false;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            logger.LogInformation("Entered Order Page");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser user = await userManager.FindByIdAsync(userId);
            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            ViewBag.BgCategories = bgCategories;
            return View(new Models.OrderInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(Models.OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order");
                return View(input);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser user = await userManager.FindByIdAsync(userId);
            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            OrderModel model = new()
            {
                Description = input.Description,
                OrderDate = DateTime.Now,
                Status = input.Status,
                Buyer = user,
                Cad = new CadModel()
                {
                    Name = input.Name,
                    Category = input.Category,
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

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            OrderModel model = await orderService.GetByIdAsync(cadId, userId);


            if (userId != model.BuyerId)
            {
                return Unauthorized();
            }

            IdentityUser user = await userManager.FindByIdAsync(userId);
            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            Models.OrderInputModel input = new()
            {
                Status = model.Status,
                CadId = model.CadId,
                Name = model.Cad.Name,
                Category = model.Cad.Category,
                Description = model.Description,
                OrderDate = model.OrderDate,
            };

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderInputModel input, int cadId)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid Order");
                return View(input);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            OrderModel model = await orderService.GetByIdAsync(cadId, userId);

            if (userId != model.BuyerId)
            {
                return Unauthorized();
            }

            IdentityUser user = await userManager.FindByIdAsync(userId);
            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            model.Cad.Name = input.Name;
            model.Description = input.Description;
            model.Cad.CategoryId = input.CategoryId;
            await orderService.EditAsync(model);

            logger.LogInformation("Edited Order");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int cadId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IdentityUser user = await userManager.FindByIdAsync(userId);

            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized();
            }

            await orderService.DeleteAsync(cadId, userId);

            return RedirectToAction(nameof(Index));
        }
    }
}
