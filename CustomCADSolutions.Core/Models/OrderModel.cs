using CustomCADSolutions.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Core.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CadId { get; set; }

        [Required]
        public int BuyerId { get; set; }

        [Required(ErrorMessage = "Order Description is required")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Order Name length must be between 10 and 5000 characters")]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }

        [ForeignKey(nameof(CadId))]
        public CadModel Cad { get; set; } = null!;

        [ForeignKey(nameof(BuyerId))]
        public UserModel Buyer { get; set; } = null!;
    }
}
