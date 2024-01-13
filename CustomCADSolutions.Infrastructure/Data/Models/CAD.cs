using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Cad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public DateTime? CreationDate { get; set; }

        [Required]  
        public string Url { get; set; } = null!;
        
        [Required]
        public Category Category { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
