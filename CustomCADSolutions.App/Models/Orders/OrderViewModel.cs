using System.Text.Json.Serialization;

namespace CustomCADSolutions.App.Models.Orders
{
    public class OrderViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("category")]
        public string Category { get; set; } = null!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("orderDate")]
        public string OrderDate { get; set; } = null!;

        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;

        [JsonPropertyName("cadId")]
        public int CadId { get; set; }

        [JsonPropertyName("buyerId")]
        public string BuyerId { get; set; } = null!;

        [JsonPropertyName("buyerName")]
        public string BuyerName { get; set; } = null!;
    }
}
