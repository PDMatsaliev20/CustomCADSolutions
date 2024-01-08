using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Infrastructure.Data.DataProcessor.ImportDtos
{
    public class ImportCADModel
    {
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"[\w]{32}")]
        [JsonProperty("Url")]
        public string URL { get; set; } = null!;
    }
}