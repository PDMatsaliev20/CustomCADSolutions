using CustomCADSolutions.App.Models;
using CustomCADSolutions.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomCADSolutions.App.Controllers
{
    public class DeepAIController : Controller
    {
        private readonly IDeepAIService service;

        public DeepAIController(IDeepAIService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult GenerateImage()
        {
            ImageInputModel model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateImage([FromBody] ImageInputModel model)
        {
            string image = await service.GenerateImage(model.Description);
            TempData["ImageURL"] = image;
            return RedirectToAction("ImageGenerated");  
        }

        public IActionResult ImageGenerated()
        {
            string json = (TempData["ImageURL"] as string)!;

            if (string.IsNullOrEmpty(json))
            {
                return View("ImageGenerated", string.Empty);
            }

            ImageViewModel result = JsonConvert.DeserializeObject<ImageViewModel>(json)!;
            string imageUrl = result.ImageUrl;
            
            return View("ImageGenerated");
        }
    }
}
