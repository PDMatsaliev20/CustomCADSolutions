using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Components
{
    public class FooterComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(
            string privacy,
            string contacts)
        {
            ViewBag.Privacy = privacy;
            ViewBag.Contacts = contacts;

            return await Task.FromResult<IViewComponentResult>(View());
        }
    }
}
