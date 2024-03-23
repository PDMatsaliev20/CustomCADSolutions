using CustomCADSolutions.App.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Components
{
    public class OrdersComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(OrderViewModel order) 
        {
            return await Task.FromResult<IViewComponentResult>(View(order.Status, order));
        }
    }
}
