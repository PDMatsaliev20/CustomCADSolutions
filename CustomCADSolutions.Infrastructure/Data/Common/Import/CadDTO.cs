using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static CustomCADSolutions.Infrastructure.Constants.DataConstants;

namespace CustomCADSolutions.Infrastructure.Data.Common.Import
{
    public class CadDTO
    {
        [Required]
        [StringLength(Cad.NameMaxLength, MinimumLength = Cad.NameMinLength)]
        [JsonPropertyName("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(Cad.UrlRegExPattern)]
        [JsonPropertyName("Url")]
        public string Url { get; set; } = null!;
    }
}
