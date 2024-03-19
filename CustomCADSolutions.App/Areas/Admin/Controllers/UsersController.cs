using CustomCADSolutions.App.Models.Users;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ICadService cadService;

        public UsersController(
            UserManager<IdentityUser> userManager,
            ICadService cadService,
            ILogger<UsersController> logger)
        {
            this.userManager = userManager;
            this.cadService = cadService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            UserViewModel[] views = (await userManager.Users
                .ToArrayAsync())
                .Select(async u => new UserViewModel()
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Role = string.Join(", ", (await userManager.GetRolesAsync(u)).FirstOrDefault()!),
                    Email = u.Email,
                    Phone = u.PhoneNumber
                })
                .Select(t => t.Result)
                .ToArray();

            ViewBag.Roles = new[] { "Administrator", "Designer", "Contributer", "Client" };
            ViewBag.RolesBg = new[] { "Администратор", "Дизайнер", "Помощник", "Клиент" };

            return View(views);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRole(string username, string selectedRole)
        {
            IdentityUser user = await userManager.FindByNameAsync(username);
            var roles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRoleAsync(user, roles.Single());
            await userManager.AddToRoleAsync(user, selectedRole);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string username)
        {
            IdentityUser user = await userManager.FindByNameAsync(username);

            foreach (CadModel model in (await cadService.GetAllAsync()).Where(c => c.CreatorId == user.Id))
            {
                RedirectToAction(nameof(CadsController.Delete), "Cad", new { id = model.Id });
            }
            await userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
