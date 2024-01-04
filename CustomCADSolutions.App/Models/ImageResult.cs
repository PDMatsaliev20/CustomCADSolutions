using Newtonsoft.Json;

namespace CustomCADSolutions.App.Controllers
{
    public class ImageResult
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;
        [JsonProperty("output_url")]
        public string ImageUrl { get; set; } = null!;
    }
}