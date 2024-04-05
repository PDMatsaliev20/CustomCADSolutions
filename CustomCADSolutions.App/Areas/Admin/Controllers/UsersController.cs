using CustomCADSolutions.App.Mappings.CadDTOs;
using CustomCADSolutions.App.Models.Users;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomCADSolutions.App.Extensions;
using AutoMapper;
using CustomCADSolutions.App.Mappings;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly HttpClient httpClient;

        public UsersController(
            UserManager<IdentityUser> userManager,
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
