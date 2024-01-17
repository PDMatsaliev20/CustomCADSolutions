using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Core.Models
{
    public class CadModel
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

        [Required]
        public ICollection<OrderModel>? Orders { get; set; }
    }
}
