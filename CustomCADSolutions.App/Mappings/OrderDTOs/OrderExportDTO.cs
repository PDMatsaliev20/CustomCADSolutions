using CustomCADSolutions.App.Mappings.CadDTOs;
using System.Text.Json.Serialization;

namespace CustomCADSolutions.App.Mappings.DTOs
{
    public class OrderExportDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("buyerId")]
        public string BuyerId { get; set; } = null!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("shouldShow")]
        public bool ShouldShow { get; set; } = true;

        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;
        
        [JsonPropertyName("orderDate")]
        public string OrderDate { get; set; } = null!;

        [JsonPropertyName("cadId")]
        public int CadId { get; set; }

        [JsonPropertyName("buyerName")]
        public string BuyerName { get; set; } = null!;

        [JsonPropertyName("cad")]
        public CadExportDTO Cad { get; set; } = null!;
    }
}
