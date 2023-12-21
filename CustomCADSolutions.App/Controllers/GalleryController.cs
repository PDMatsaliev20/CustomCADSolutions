using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Show()
        {
            return View();
        }
    }
}
