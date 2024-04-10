using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.App.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Components
{
    public class CadComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string button, bool alreadyOrdered)
        {
            string view = string.Empty;
            switch (button)
            {
                case "Edit": view = "EditButton"; break;
                case "Delete": view = "DeleteButton"; break;
                case "Colorize": view = "ColorizeButton"; break;
                case "Download": view = "DownloadButton"; break;
                case "Validate": view = "ValidateButton"; break;
                case "Order": view = alreadyOrdered ? "CannottOrderButton" : "OrderButton"; break;
            }
            return await Task.FromResult<IViewComponentResult>(View(view));
        }
    }
}
