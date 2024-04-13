using System.Text.Json.Serialization;

namespace CustomCADSolutions.Core.Mappings.DTOs
{
    public class OrderExportDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        
        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;
        
        [JsonPropertyName("orderDate")]
        public string OrderDate { get; set; } = null!;

        [JsonPropertyName("cadId")]
        public int? CadId { get; set; }
        
        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; } = null!;

        [JsonPropertyName("buyerName")]
        public string BuyerName { get; set; } = null!;
    }
}
