using CustomCADSolutions.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Order
    {
        [Key]
        [Comment("Identification of Order")]
        public int Id { get; set; }

        [Required]
        [Comment("Identification of 3D model")]
        public int CadId { get; set; }

        [Required]
        [Comment("Identification of User")]
        public int BuyerId { get; set; }

        [Required]
        [Comment("Description of Order")]
        [MaxLength(DataConstants.Order.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [Comment("Date of Order")]
        public DateTime OrderDate { get; set; }

        [ForeignKey(nameof(CadId))]
        public Cad Cad { get; set; } = null!;

        [ForeignKey(nameof(BuyerId))]
        public User Buyer { get; set; } = null!;
    }
}
