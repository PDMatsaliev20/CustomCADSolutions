using CustomCADSolutions.App.Models.Roles;
using CustomCADSolutions.App.Models.Users;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class RolesController : Controller
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            RoleIndexViewModel[] roles = await roleManager.Roles
                .Select(r => new RoleIndexViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                })
                .OrderBy(r => r.Name)
                .ToArrayAsync();

            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            AppRole role = await roleManager.FindByIdAsync(id);
            IList<AppUser> users = await userManager.GetUsersInRoleAsync(role.Name);

            RoleDetailsViewModel view = new()
            {
                Id = role.Id,
                Name = role.Name,
                Users = users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email,
                    Phone = u.PhoneNumber,
                })
                .ToArray(),
            };

            return View(view);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new AddRoleModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddRoleModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            try
            {
                await roleManager.CreateAsync(new() { Name = input.Name });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            AppRole role = await roleManager.FindByIdAsync(id);
            EditRoleModel input = new()
            {
                Id = role.Id,
                Name = role.Name,
            };

            return View(input);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditRoleModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            try
            {
                AppRole role = await roleManager.FindByIdAsync(id);
                role.Name = input.Name;
                await roleManager.UpdateAsync(role);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                AppRole role = await roleManager.FindByIdAsync(id);
                var response = await roleManager.DeleteAsync(role);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
