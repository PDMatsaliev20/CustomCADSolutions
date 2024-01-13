using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Import
{
    public class ImportCadDTO
    {
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"[\w]{32}")]
        [JsonProperty("Url")]
        public string URL { get; set; } = null!;

        [Required]
        [JsonProperty("Category")]
        public Category Category { get; set; }
    }
}