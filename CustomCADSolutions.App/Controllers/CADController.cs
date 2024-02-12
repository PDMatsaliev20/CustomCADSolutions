using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CustomCADSolutions.App.Controllers
{
    [Authorize]
    public class CadController : Controller
    {
        private readonly ICadService cadService;
        private readonly ILogger logger;
        private readonly UserManager<IdentityUser> userManager;

        public CadController(
            ICadService cadService, 
            ILogger<CadModel> logger,
            UserManager<IdentityUser> userManager)
        {
            this.cadService = cadService;
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<CadModel> models = (await cadService.GetAllAsync()).Where(cad => cad.CreatorId == creatorId);

            IEnumerable<CadViewModel> views = models
                .Select((model) =>
                {
                    CadViewModel view = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Category = model.Category.ToString(),
                        CreationDate = model.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    };

                    return view;
                });

            return View(views);
        }

        [HttpGet]
        public IActionResult Add()
        {
            logger.LogInformation("Entered Submit Page");
            return View(new CadInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(CadInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid 3d Model");
                return View(input);
            }

            string creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            using MemoryStream stream = new();
            input.File.CopyTo(stream);
            CadModel model = new()
            {
                Name = input.Name,
                Category = input.Category,
                CreationDate = DateTime.Now,
                CadInBytes = stream.ToArray(),
                CreatorId = creatorId,
                Creator = await userManager.FindByIdAsync(creatorId)
            };
            await cadService.CreateAsync(model);

            if (input.File == null || input.File.Length <= 0)
            {
                return BadRequest("Dumb 3d model");
            }

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot\others\cads\{input.Name}.stl");
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await input.File.CopyToAsync(fileStream);

            logger.LogInformation("Submitted 3d Model");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CadModel cad = await cadService.GetByIdAsync(id);
            IdentityUser? creator = cad.Creator;

            if (creator == null)
            {
                return BadRequest();
            }

            if (creator.UserName != User.FindFirstValue(ClaimTypes.Name))
            {
                return Unauthorized();
            }

            CadInputModel input = new()
            {
                Name = cad.Name,
                Category = cad.Category,
            };

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CadInputModel input, int id)
        {
            CadModel model = await cadService.GetByIdAsync(id);
            IdentityUser? creator = model.Creator;

            if (creator == null)
            {
                return BadRequest();
            }

            if (creator.UserName != User.FindFirstValue(ClaimTypes.Name))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            using MemoryStream stream = new();
            input.File.CopyTo(stream);
            model.CadInBytes = stream.ToArray();
            model.Name = input.Name;
            model.Category = input.Category;

            await cadService.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CadModel cad = await cadService.GetByIdAsync(id);

            if (cad.Creator == null)
            {
                return BadRequest();
            }

            if (cad.Creator.UserName != User.FindFirstValue(ClaimTypes.Name))
            {
                return Unauthorized();
            }

            await cadService.DeleteAsync(cad.Id);

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot\others\cads\{cad.Name}.stl");
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);    
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
