using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using CustomCADSolutions.Core.Contracts;

namespace CustomCADSolutions.Api
{
    [ApiController]
    [Route("[controller]")]
    public class DalleController : ControllerBase
    {
        private readonly IOpenAIService openAIService;

        public DalleController(IOpenAIService openAIService)
        {
            this.openAIService = openAIService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateImage([FromBody] string prompt)
        {
            var imageResponse = await this.openAIService.CallDALLEAsync(prompt);
            if (imageResponse != null)
            {
                return Ok(imageResponse);
            }
            else
            {
                return BadRequest("Unable to generate image");
            }
        }
    }
}
