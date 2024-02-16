using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CustomCADSolutions.App.Controllers
{
    [Authorize(Roles = "Contributer,Administrator")]
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
                        Coords = model.Coords,
                        SpinAxis = model.SpinAxis,
                        SpinFactor = model.SpinFactor,
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

            if (input.CadFile == null || input.CadFile.Length <= 0)
            {
                return BadRequest("Dumb 3d model");
            }

            CadModel model = new()
            {
                Name = input.Name,
                Category = input.Category,
                CreationDate = DateTime.Now,
                CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Coords = (input.X, input.Y, input.Z),
                SpinAxis = input.SpinAxis,
                SpinFactor = input.SpinFactor / 100d,
            };
            model.Creator = await userManager.FindByIdAsync(model.CreatorId);

            int cadId = await cadService.CreateAsync(model);

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/others/cads/{input.Name}{cadId}.stl");
            using FileStream fileStream = new(filePath, FileMode.Create);
            await input.CadFile.CopyToAsync(fileStream);

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
                X = cad.Coords.Item1,
                Y = cad.Coords.Item2,
                Z = cad.Coords.Item3,
                SpinAxis = cad.SpinAxis,
                SpinFactor = (int)(cad.SpinFactor * 100),
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

            if (input.CadFile != null && !ModelState.IsValid)
            {
                logger.LogError("Did NOT Save Model Changes");

                if (input.CadFile == null)
                {
                    logger.LogError("Method above DID NOT work");
                }

                return View(input);
            }

            model.Name = input.Name;
            model.Category = input.Category;
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

            if (cad.Creator == null)
            {
                return BadRequest();
            }

            if (cad.Creator.UserName != User.FindFirstValue(ClaimTypes.Name))
            {
                return Unauthorized();
            }

            await cadService.DeleteAsync(cad.Id);

            string filePath = $@"wwwroot\others\cads\{cad.Name}{cad.Id}.stl";
            string fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            if (System.IO.File.Exists(fullFilePath))
            {
                System.IO.File.Delete(fullFilePath);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
