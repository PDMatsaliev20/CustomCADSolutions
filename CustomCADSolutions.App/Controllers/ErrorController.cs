using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult StatusCodeHandler(int? statusCode = null)
        {
            if (statusCode != null)
            {
                string view = statusCode switch
                {
                    400 => "HttpError400",
                    401 => "HttpError401",
                    404 => "HttpError404",
                    500 => "HttpError500",
                    _ => "HttpError"
                };
                return View(view);
            }
            else return View("Exception");
        }
    }
}
