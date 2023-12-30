using CustomCADSolutions.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Core.Models
{
    public class CADModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime CreationDate { get; set; }

        public string? Description { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
