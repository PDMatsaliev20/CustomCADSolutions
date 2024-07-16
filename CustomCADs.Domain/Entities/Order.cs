using CustomCADs.Domain.Entities.Enums;
using CustomCADs.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.Domain.Entities
{
    public class Order
    {
        [Key]
        [Comment("Identitfication of Order")]
        public int Id { get; set; }

        [Required]
        [Comment("Name of Order")]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("Description of Order")]
        [MaxLength(OrderConstants.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [Comment("Date of Order")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Comment("Status of Order")]
        public OrderStatus Status { get; set; }

        [Required]
        [Comment("Should Order Be Visible After Completion")]
        public bool ShouldShow { get; set; }

        [Comment("Identification of Order's Category")]
        public int CategoryId { get; set; }

        [Comment("Identification of Orders' 3D Model")]
        public int? CadId { get; set; }

        [Required]
        [Comment("Identification of User")]
        public string BuyerId { get; set; } = null!;

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [ForeignKey(nameof(CadId))]
        public Cad? Cad { get; set; }

        [ForeignKey(nameof(BuyerId))]
        public AppUser Buyer { get; set; } = null!;
    }
}
