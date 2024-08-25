using CustomCADs.Domain.Enums;
using CustomCADs.Domain.Identity;
using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.Domain.Entities
{
    public class Order 
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(OrderConstants.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public bool ShouldBeDelivered { get; set; }

        public string? ImagePath { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        public string BuyerId { get; set; } = null!;
        public AppUser Buyer { get; set; } = null!;
        
        public string? DesignerId { get; set; }
        public AppUser? Designer { get; set; } 

        public int? CadId { get; set; }
        public Cad? Cad { get; set; }        
    }
}
