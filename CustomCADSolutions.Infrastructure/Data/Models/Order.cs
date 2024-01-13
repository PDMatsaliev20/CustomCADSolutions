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
        [Key]
        public int Id { get; set; }

        [Required]
        public int CadId { get; set; }

        [Required]
        public int BuyerId { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }

        [ForeignKey(nameof(CadId))]
        public Cad Cad { get; set; } = null!;

        [ForeignKey(nameof(BuyerId))]
        public User Buyer { get; set; } = null!;
    }
}
