using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Components
{
    public class CadComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int id, string name, string button, string color = "#000000")
        {
            ViewBag.Id = id;
            ViewBag.Name = name;
            ViewBag.Color = color;

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
