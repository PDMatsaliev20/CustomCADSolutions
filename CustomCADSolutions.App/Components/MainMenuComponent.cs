using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Components
{
    public class MainMenuComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string[] roles = { "Client", "Contributor", "Designer", "Admin" };

            string view = string.Empty;
            if (User.Identity?.IsAuthenticated ?? false)
            {
                foreach (string role in roles)
                {
                    if (User.IsInRole(role))
                    {
                        view = role;
                    }
                }
            }
            else
            {
                view = "Unauthenticated";
            }

            return await Task.FromResult<IViewComponentResult>(View(view));
        }
    }
}
