using CustomCADs.App.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.App.Components
{
    public class AllOrdersComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(OrderViewModel order, string status)
        {
            ViewBag.Id = order.Id;
            ViewBag.Name = order.Name;
            return await Task.FromResult<IViewComponentResult>(View(status));
        }
    }
}
