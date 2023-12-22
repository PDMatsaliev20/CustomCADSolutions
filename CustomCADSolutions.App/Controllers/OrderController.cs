using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace CustomCADSolutions.App.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Description(string description)
        {
            
            return View();
        }

        public IActionResult Sent()
        {
            return View();
        }
    }
}
