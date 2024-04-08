using CustomCADSolutions.App.Mappings.CadDTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomCADSolutions.App.Mappings.DTOs
{
    public class OrderImportDTO
    {
        [JsonPropertyName("id")]
        [Required]
        public int Id { get; set; }
        
        [JsonPropertyName("cadId")]
        [Required]
        public int CadId { get; set; }

        [JsonPropertyName("buyerId")]
        [Required]
        public string BuyerId { get; set; } = null!;

        [JsonPropertyName("description")]
        [Required(ErrorMessage = "Order Description is required")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Order Name length must be between 10 and 5000 characters")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("status")]
        [Required]
        public string Status { get; set; } = null!;

        [JsonPropertyName("cad")]
        public CadImportDTO Cad { get; set; } = null!;
    }
}
