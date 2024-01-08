using CustomCADSolutions.App.Models;
using CustomCADSolutions.App.Modelss;
using CustomCADSolutions.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomCADSolutions.App.Controllers
{
    public class DeepAIController : Controller
    {
        private readonly IDeepAIService service;
        private readonly ILogger logger;

        public DeepAIController(IDeepAIService service, ILogger<DeepAIController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GenerateImage()
        {
            string description = string.Empty;
            return View(model : description);
        }

        [HttpPost]
        public async Task<ActionResult> ImageGenerated([FromBody] string description)
        {
            string imageUrl = await service.GenerateImage(description);
            TempData["ImageURL"] = imageUrl;
            return View();  
        }

        [HttpGet]
        public IActionResult ImageGenerated()
        {
            string json = (TempData["ImageURL"] as string)!;

            if (string.IsNullOrEmpty(json))
            {
                return View("ImageGenerated", string.Empty);
            }

            ImageViewModel result = JsonConvert.DeserializeObject<ImageViewModel>(json)!;
            string imageUrl = result.ImageUrl;
            
            return View("ImageGenerated", imageUrl);
        }
    }
}
