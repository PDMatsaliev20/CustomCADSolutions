using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Order
    {
        [Required]
        public int CADId { get; set; }
        
        [Required]
        public int BuyerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [ForeignKey(nameof(CADId))]
        public CAD CAD { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(BuyerId))]
        public User Buyer { get; set; } = null!;
    }
}
