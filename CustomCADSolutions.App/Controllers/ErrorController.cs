using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Controllers
{
    public class ErrorController : Controller
    {

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult StatusCodeHandler(int? statusCode = null)
        {
            if (statusCode == null)
            {
                return View("Exception");
            }
            else
            {
                return View($"HttpError{statusCode}");
            }
        }
    }
}
