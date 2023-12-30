using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace CustomCADSolutions.App.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService service;

        public OrderController(IOrderService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            CADModel model = new();
            ViewBag.CAD = (await service.GetCADsAsync()).Select(t => new SelectListItem(t.Name, t.Id.ToString())); ;
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(OrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            service.CreateAsync(model);
            
            return RedirectToAction("Sent");
        }

        public IActionResult Sent()
        {   
            return View();
        }
    }
}
