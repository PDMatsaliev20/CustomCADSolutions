using CustomCADSolutions.App.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> logger;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(
            UserManager<IdentityUser> userManager,
            ILogger<UserController> logger)
        {
            this.userManager = userManager;
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

            await userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
