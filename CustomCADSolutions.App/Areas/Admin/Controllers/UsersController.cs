using CustomCADSolutions.App.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> logger;
        private readonly UserManager<AppUser> userManager;
        private readonly HttpClient httpClient;

        public UsersController(
            UserManager<AppUser> userManager,
            HttpClient httpClient,
            ILogger<UsersController> logger)
        {
            this.userManager = userManager;
            this.httpClient = httpClient;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            UserViewModel[] views = (await userManager.Users.ToArrayAsync())
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
            
            ViewBag.Roles = new string[] { "Administrator", "Designer", "Contributor", "Client" };
            return View(views);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRole(string username, string selectedRole)
        {
            AppUser user = await userManager.FindByNameAsync(username);
            var roles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRoleAsync(user, roles.Single());
            await userManager.AddToRoleAsync(user, selectedRole);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string username)
        {
            AppUser user = await userManager.FindByNameAsync(username);
            await userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
