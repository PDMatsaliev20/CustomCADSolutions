using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CustomCADSolutions.Core.Models
{
    public class OrderModel
    {
        [Required]
        public int CadId { get; set; }

        [Required]
        public string BuyerId { get; set; } = null!;

        [Required(ErrorMessage = "Order Description is required")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Order Name length must be between 10 and 5000 characters")]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }

        [ForeignKey(nameof(CadId))]
        public CadModel Cad { get; set; } = null!;

        [ForeignKey(nameof(BuyerId))]
        public IdentityUser Buyer { get; set; } = null!;
    }
}
