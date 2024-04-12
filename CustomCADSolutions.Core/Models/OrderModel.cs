using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.Text.Json.Serialization;
using CustomCADSolutions.Infrastructure.Data.Models;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using static CustomCADSolutions.Infrastructure.Data.DataConstants.OrderConstants;

namespace CustomCADSolutions.Core.Models
{
    public class OrderModel
    {
        [JsonPropertyName("id")]
        [Required]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        [JsonPropertyName("description")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, 
            ErrorMessage = LengthErrorMessage)]
        public string Description { get; set; } = null!;

        [JsonPropertyName("orderDate")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("status")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        public OrderStatus Status { get; set; }

        [JsonPropertyName("shouldShow")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        public bool ShouldShow { get; set; } = true;

        [JsonPropertyName("cadId")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int? CadId { get; set; }
        
        [JsonPropertyName("categoryId")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; set; }

        [JsonPropertyName("buyerId")]
        [Required(ErrorMessage = RequiredErrorMessage)]
        public string BuyerId { get; set; } = null!;

        [JsonPropertyName("category")]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [JsonPropertyName("cad")]
        [ForeignKey(nameof(CadId))]
        public CadModel? Cad { get; set; }
            
        [JsonPropertyName("buyer")]
        [ForeignKey(nameof(BuyerId))]
        public AppUser Buyer { get; set; } = null!;
    }
}
