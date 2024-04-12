using CustomCADSolutions.App.Mappings.CadDTOs;
using System.Text.Json.Serialization;

namespace CustomCADSolutions.App.Mappings.DTOs
{
    public class OrderExportDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        
        [JsonPropertyName("buyerId")]
        public string BuyerId { get; set; } = null!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;
        
        [JsonPropertyName("orderDate")]
        public string OrderDate { get; set; } = null!;

        [JsonPropertyName("cadId")]
        public int? CadId { get; set; }
        
        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; } = null!;

        [JsonPropertyName("buyerName")]
        public string BuyerName { get; set; } = null!;
    }
}
