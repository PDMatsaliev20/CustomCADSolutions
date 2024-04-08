using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.Text.Json.Serialization;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Core.Models
{
    public class OrderModel
    {
        [JsonPropertyName("id")]
        [Required]
        public int Id { get; set; }

        [JsonPropertyName("description")]
        [Required(ErrorMessage = "Order Description is required")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Order Name length must be between 10 and 5000 characters")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("orderDate")]
        [Required]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("status")]
        [Required]
        public OrderStatus Status { get; set; }

        [JsonPropertyName("shouldShow")]
        [Required]
        public bool ShouldShow { get; set; } = true;

        [JsonPropertyName("cadId")]
        [Required]
        public int CadId { get; set; }

        [JsonPropertyName("buyerId")]
        [Required]
        public string BuyerId { get; set; } = null!;

        [JsonPropertyName("cad")]
        [ForeignKey(nameof(CadId))]
        public CadModel Cad { get; set; } = null!;

        [JsonPropertyName("buyer")]
        [ForeignKey(nameof(BuyerId))]
        public AppUser Buyer { get; set; } = null!;
    }
}
