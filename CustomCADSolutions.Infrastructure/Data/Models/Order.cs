using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Order
    {
        [Required]
        [Comment("Identification of 3D model")]
        public int CadId { get; set; }

        [Required]
        [Comment("Identification of User")]
        public string BuyerId { get; set; } = null!;

        [Required]
        [Comment("Description of Order")]
        [MaxLength(DataConstants.OrderConstants.DescriptionMaxLength)]
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

        [ForeignKey(nameof(CadId))]
        public Cad Cad { get; set; } = null!;

        [ForeignKey(nameof(BuyerId))]
        public IdentityUser Buyer { get; set; } = null!;
    }
}
