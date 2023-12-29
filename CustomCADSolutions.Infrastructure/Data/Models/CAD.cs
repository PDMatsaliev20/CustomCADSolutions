using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class CAD
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        public string? Customer { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public string? Description { get; set; }
    }
}
