using CustomCADs.App.Models.Users;
using CustomCADs.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class UsersController(
        UserManager<AppUser> userManager,
        ILogger<UsersController> logger) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            UserViewModel[] views = (await userManager.Users.ToArrayAsync())
                .Select(async u => new UserViewModel()
                {
                    Id = u.Id,
                    Username = u.UserName!,
                    Role = string.Join(", ", (await userManager.GetRolesAsync(u)).FirstOrDefault()!),
                    Email = u.Email!,
                    Phone = u.PhoneNumber
                })
                .Select(t => t.Result)
                .ToArray();
            
            string[] roles = [
                RoleConstants.Admin,
                RoleConstants.Designer,
                RoleConstants.Contributor,
                RoleConstants.Client
            ];
            ViewBag.Roles = roles;
            return View(views);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRole(string username, string selectedRole)
        {
            try
            {
                AppUser user = await userManager.FindByNameAsync(username)
                ?? throw new KeyNotFoundException();
                var roles = await userManager.GetRolesAsync(user);

                await userManager.RemoveFromRoleAsync(user, roles.Single());
                await userManager.AddToRoleAsync(user, selectedRole);

                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return Unauthorized();
            }
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
