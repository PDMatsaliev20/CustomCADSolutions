using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomCADSolutions.App.Controllers
{
    [Authorize(Roles = "Designer")]
    public class DesignerCadController : Controller
    {
        private readonly ICadService cadService;
        private readonly IOrderService orderService;
        private readonly ICategoryService categoryService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public DesignerCadController(
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
        public async Task<IActionResult> ContributerCads()
        {
            IEnumerable<CadViewModel> views = (await cadService.GetAllAsync())
                .Where(m => m.Creator != null && !m.Validated)
                .Select(m => new CadViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Category = m.Category.Name,
                    CreationDate = m.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    Coords = m.Coords,
                    SpinAxis = m.SpinAxis,
                    SpinFactor = m.SpinFactor,
                    CreatorName = m.Creator!.UserName,
                });

            return View(views);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateCad(int cadId)
        {
            CadModel model = await cadService.GetByIdAsync(cadId);

            model.Validated = true;
            await cadService.EditAsync(model);

            return RedirectToAction(nameof(ContributerCads));
        }
    }
}
