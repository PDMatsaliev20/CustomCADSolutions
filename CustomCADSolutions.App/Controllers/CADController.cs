using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.App.Controllers
{
    [Authorize(Roles = "Contributer,Designer")]
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
            IEnumerable<CadViewModel> views = (await cadService.GetAllAsync())
                .Where(model => model.CreatorId == GetUserId())
                .Select(model => new CadViewModel
                {
                    Id = model.Id,
                    Name = model.Name,
                    Category = model.Category.Name,
                    CreationDate = model.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    Coords = model.Coords,
                    SpinAxis = model.SpinAxis,
                    SpinFactor = model.SpinFactor,
                    Validated = model.Validated,
                });

            return View(views);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            logger.LogInformation("Entered Submit Page");

            CadInputModel input = new() { Categories = await GetCategories() };
            return View(input);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(CadInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid 3d Model");
                input.Categories = await GetCategories();
                if (ModelState.ErrorCount > 1)
                {
                    return View(input);
                }
            }

            if (input.CadFile == null || input.CadFile.Length <= 0)
            {
                return BadRequest("Invalid 3d model");
            }

            CadModel model = new()
            {
                Name = input.Name,
                CategoryId = input.CategoryId,
                Validated = User.IsInRole("Designer"),
                CreationDate = DateTime.Now,
                CreatorId = GetUserId(),
            };
            model.Creator = await userManager.FindByIdAsync(model.CreatorId);
            int cadId = await cadService.CreateAsync(model);
            
            string filePath = GetCadPath(input.Name, cadId);
            using FileStream fileStream = new(filePath, FileMode.Create);
            await input.CadFile.CopyToAsync(fileStream);

            logger.LogInformation("Submitted 3d Model");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);

            if (model.Creator == null)
            {
                return BadRequest();
            }

            if (model.CreatorId != GetUserId())
            {
                return Unauthorized();
            }

            CadInputModel input = new()
            {
                Categories = await GetCategories(),
                Name = model.Name,
                CategoryId = model.CategoryId,
                X = model.Coords.Item1,
                Y = model.Coords.Item2,
                Z = model.Coords.Item3,
                SpinAxis = model.SpinAxis,
                SpinFactor = (int)(model.SpinFactor * 100),
            };

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CadInputModel input, int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);

            if (model.Creator == null)
            {
                return BadRequest();
            }

            if (model.CreatorId != GetUserId())
            {
                return Unauthorized();
            }

            if (input.Name != model.Name)
            {
                string source = GetCadPath(model.Name, id);
                string destination = GetCadPath(input.Name, id);

                System.IO.File.Move(source, destination);
                model.Name = input.Name;
            }

            model.CategoryId = input.CategoryId;
            model.Coords = (input.X, input.Y, input.Z);
            model.SpinAxis = input.SpinAxis;
            model.SpinFactor = input.SpinFactor / 100d;

            await cadService.EditAsync(model);
            logger.LogInformation("Saved Model Changes");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CadModel cad = await cadService.GetByIdAsync(id);

            if (cad.CreatorId != GetUserId())
            {
                return Unauthorized();
            }

            OrderModel[] orders = (await orderService.GetAllAsync()).Where(o => o.CadId == cad.Id).ToArray();
            orders.ToList().ForEach(o => o.Status = OrderStatus.Pending);

            await cadService.DeleteAsync(cad.Id);

            string filePath = GetCadPath(cad.Name, cad.Id);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            else logger.LogWarning("File not found");

            return RedirectToAction(nameof(Index));
        }

        // Private methods

        private string GetCadPath(string cadName, int cadId) 
            => Path.Combine(hostingEnvironment.WebRootPath, "others", "cads", $"{cadName}{cadId}.stl");

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        private async Task<Category[]> GetCategories()
            => (await categoryService.GetAllAsync()).ToArray();
    }
}
