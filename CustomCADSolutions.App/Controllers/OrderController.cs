using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace CustomCADSolutions.App.Controllers
{
    public class OrderController : Controller
    {
        private ICADService service;

        public OrderController(ICADService service)
        {
            this.service = service;
        }

        public IActionResult Description()
        {
            return View();
        }

        public IActionResult Sent()
        {
            return View();
        }
    }
}
