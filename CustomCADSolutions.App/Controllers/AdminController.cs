using CustomCADSolutions.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.App.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> logger;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(
            UserManager<IdentityUser> userManager,
            ILogger<AdminController> logger)
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
