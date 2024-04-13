using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using static CustomCADSolutions.Infrastructure.Data.DataConstants.CadConstants;

namespace CustomCADSolutions.Core.Mappings.CadDTOs
{
    public class CadImportDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, 
            ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        [JsonPropertyName("isValidated")]
        public bool IsValidated { get; set; }

        [JsonPropertyName("spinAxis")]
        [RegularExpression(SpinAxisRegEx, ErrorMessage = SpinAxisErrorMessage)]
        public char? SpinAxis { get; set; }

        [JsonPropertyName("categoryId")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; set; }
        
        [JsonPropertyName("price")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        public decimal Price { get; set; }

        [JsonPropertyName("creatorId")]
        public string CreatorId { get; set; } = null!;

        [JsonPropertyName("coords")]
        public int[] Coords { get; set; } = new int[3];

        [JsonPropertyName("rgb")]
        public int[] RGB { get; set; } = new int[3] { 255, 255, 255 };
    }
}