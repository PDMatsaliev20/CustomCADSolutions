using CustomCADSolutions.App.Models.Cads.View;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Components
{
    public class CategoryCadComponent : ViewComponent
    {
        private readonly IOrderService orderService;
        public CategoryCadComponent(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CadViewModel cad)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(cad.Id);
                ViewBag.AlreadyOrdered = true;
            }
            catch
            {
                ViewBag.AlreadyOrdered = false;
            }
            
            if (User.IsInRole("Client"))
            {
                ViewBag.Area = "Client";
            }
            else if (User.IsInRole("Contributor"))
            {
                if (User.Identity!.Name != cad.CreatorName)
                { 
                    ViewBag.Area = "Contributor"; 
                }
                else ViewBag.AlreadyOrdered = true;
            }

            return await Task.FromResult<IViewComponentResult>(View(cad));
        }
    }
}
