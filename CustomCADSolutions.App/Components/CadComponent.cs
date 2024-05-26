using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Components
{
    public class CadComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int id, int productId, string name, string button)
        {
            ViewBag.Id = id;
            ViewBag.ProductId = productId;
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
