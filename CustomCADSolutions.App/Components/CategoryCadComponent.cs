using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Components
{
    public class CategoryCadComponent : ViewComponent
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IOrderService orderService;
        public CategoryCadComponent(
            UserManager<IdentityUser> userManager, 
            IOrderService orderService)
        {
            this.userManager = userManager;
            this.orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CadViewModel cad, string userId)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(cad.Id, userId);
                ViewBag.AlreadyOrdered = true;
            }
            catch
            {
                ViewBag.AlreadyOrdered = false;
            }

            if (User.IsInRole("Contributer") && User.Identity!.Name != cad.CreatorName!)
            {
                ViewBag.Area = "Contributer";
            }
            else if (User.IsInRole("Client"))
            {
                ViewBag.Area = "Client";
            }

            return await Task.FromResult<IViewComponentResult>(View(cad));
        }
    }
}
