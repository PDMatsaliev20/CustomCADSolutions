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

        public int? CadId { get; set; }

        [Required]
        public int BuyerId { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }

        [ForeignKey(nameof(CadId))]
        public CadModel? Cad { get; set; }

        [ForeignKey(nameof(BuyerId))]
        public UserModel Buyer { get; set; } = null!;
    }
}
