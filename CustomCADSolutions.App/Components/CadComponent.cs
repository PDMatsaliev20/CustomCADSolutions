using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Components
{
    public class CadComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int id, string name, string area, string button)
        {
            ViewBag.Id = id;
            ViewBag.Name = name;
            ViewBag.Area = area;

            string view = button switch
            {
                "Details" => "DetailsButton",
                "Edit" => "EditButton",
                "Delete" => "DeleteButton",
                "Colorize" => "ColorizeButton",
                "Download" => "DownloadButton",
                "Validate" => "ValidateButton",
                "Order" => "OrderButton",
                _ => "Default"
            };

            return View(view);
        }
    }
}
