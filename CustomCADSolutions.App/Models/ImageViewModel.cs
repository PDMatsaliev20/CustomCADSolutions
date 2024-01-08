using Newtonsoft.Json;

namespace CustomCADSolutions.App.Modelss
{
    public class ImageViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;
        [JsonProperty("output_url")]
        public string ImageUrl { get; set; } = null!;
    }
}