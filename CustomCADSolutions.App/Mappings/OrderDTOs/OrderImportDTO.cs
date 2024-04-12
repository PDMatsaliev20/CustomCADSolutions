using CustomCADSolutions.App.Mappings.CadDTOs;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using static CustomCADSolutions.Infrastructure.Data.DataConstants.OrderConstants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomCADSolutions.App.Mappings.DTOs
{
    public class OrderImportDTO
    {
        [JsonPropertyName("id")]
        [Required]
        public int Id { get; set; }

        [JsonPropertyName("description")]
        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;
        
        [JsonPropertyName("name")]
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [JsonPropertyName("status")]
        [Required]
        public string Status { get; set; } = null!;
        
        [JsonPropertyName("shouldShow")]
        [Required]
        public bool ShouldShow { get; set; }

        [JsonPropertyName("cadId")]
        public int? CadId { get; set; }
        
        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("buyerId")]
        [Required]
        public string BuyerId { get; set; } = null!;

        [JsonPropertyName("cad")]
        public CadImportDTO? Cad { get; set; } 
    }
}
