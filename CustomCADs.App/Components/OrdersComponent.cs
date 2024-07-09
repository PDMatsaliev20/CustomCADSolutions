using CustomCADs.App.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.App.Components
{
    public class OrdersComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(OrderViewModel order, bool isAdmin = false) 
        {
            var view = isAdmin ? View("Admin", order) : View(order.Status, order);
            return await Task.FromResult<IViewComponentResult>(view);
        }
    }
}
