using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CadController : Controller
    {
        private readonly ICadService cadService;
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public CadController(
            ICadService cadService,
            IOrderService orderService,
            ICategoryService categoryService,
            ILogger<CadModel> logger,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostingEnvironment)
        {
            this.cadService = cadService;
            this.orderService = orderService;
            this.categoryService = categoryService;
            this.logger = logger;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            CadViewModel[] views = (await cadService.GetAllAsync())
                .Where(c => !string.IsNullOrEmpty(c.CreatorId))
                .Select(m => new CadViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Category = m.Category.Name,
                    CreationDate = m.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatorName = m.Creator!.UserName,
                    Coords = m.Coords,
                    SpinAxis = m.SpinAxis,
                    SpinFactor = m.SpinFactor,
                    Validated = m.Validated,
                }).ToArray();

            return View(views);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CadModel cad = await cadService.GetByIdAsync(id);

            OrderModel[] orders = (await orderService.GetAllAsync()).Where(o => o.CadId == cad.Id).ToArray();
            orders.ToList().ForEach(o => o.Status = OrderStatus.Pending);
            orders.ToList().ForEach(async o => await orderService.EditAsync(o));

            await cadService.DeleteAsync(cad.Id);

            string filePath = GetCadPath(cad.Name, cad.Id);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            else logger.LogWarning("File not found");

            return RedirectToAction(nameof(Index));
        }

        private string GetCadPath(string cadName, int cadId)
            => Path.Combine(hostingEnvironment.WebRootPath, "others", "cads", $"{cadName}{cadId}.stl");

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        private async Task<Category[]> GetCategories()
            => (await categoryService.GetAllAsync()).ToArray();
    }
}
