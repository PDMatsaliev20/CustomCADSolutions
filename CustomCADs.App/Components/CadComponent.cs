using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.App.Components
{
    public class CadComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int id, string name, string button)
        {
            ViewBag.Id = id;
            ViewBag.Name = name;

            string view = button switch
            {
                "Details" => "DetailsButton",
                "Edit" => "EditButton",
                "Delete" => "DeleteButton",
                "Download" => "DownloadButton",
                "Validate" => "ValidateButton",
                "Order" => "OrderButton",
                _ => "Default"
            };

            return View(view);
        }
    }
}
