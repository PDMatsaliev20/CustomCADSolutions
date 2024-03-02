using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Areas.Identity.Models;
using Microsoft.AspNetCore.Authentication;

namespace CustomCADSolutions.App.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserStore<IdentityUser> userStore;
        private readonly IUserEmailStore<IdentityUser> emailStore;

        public AccountController(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.userStore = userStore;
            this.signInManager = signInManager;
            this.emailStore = (IUserEmailStore<IdentityUser>)this.userStore;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel model, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            model.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await userManager.FindByEmailAsync(model.Email) != null)
            {
                ViewData["IsEmailUnique"] = false;
                return View();
            }

            IdentityUser user = new();

            await userStore.SetUserNameAsync(user, model.Username, CancellationToken.None);
            await emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View(new LoginInputModel());
        }

        public async Task<IActionResult> Login(LoginInputModel input, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return View();
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await signInManager.PasswordSignInAsync(input.Username, input.Password, input.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await signInManager.SignOutAsync();
            
            if (returnUrl == null)
            {
                return RedirectToAction();
            }
            return LocalRedirect(returnUrl);
            
        }
    }
}
