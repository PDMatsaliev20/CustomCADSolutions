using CustomCADSolutions.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;

namespace CustomCADSolutions.App.Mappings.CadDTOs
{
    public class CadImportDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(CadConstants.NameMaxLength, MinimumLength = CadConstants.NameMinLength, ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        [JsonPropertyName("isValidated")]
        public bool IsValidated { get; set; }

        [JsonPropertyName("spinAxis")]
        [RegularExpression(CadConstants.SpinAxisRegEx, ErrorMessage = CadConstants.SpinAxisErrorMessage)]
        public char? SpinAxis { get; set; }

        [JsonPropertyName("bytes")]
        public byte[]? Bytes { get; set; }

        [JsonPropertyName("coords")]
        public int[] Coords { get; set; } = new int[3];

        [JsonPropertyName("categoryId")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; set; }
    }
}