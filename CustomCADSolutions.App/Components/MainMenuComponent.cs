using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Components
{
    public class MainMenuComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(
            string home,
            string explore,
            string urOrders,
            string urCads,
            string allOrders,
            string allCads,
            string allUsers)
        {
            ViewBag.Home = home;
            ViewBag.Explore = explore;
            ViewBag.UrOrders = urOrders;
            ViewBag.UrCads = urCads;
            ViewBag.AllOrders = allOrders;
            ViewBag.AllCads = allCads;
            ViewBag.AllUsers = allUsers;

            return await Task.FromResult<IViewComponentResult>(View());
        }
    }
}
