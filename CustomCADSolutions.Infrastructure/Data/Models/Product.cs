using Microsoft.EntityFrameworkCore;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Product
    {
        [Key]
        [Comment("Identification of Product")]
        public int Id { get; set; }

        [Required]
        [MaxLength(CadConstants.NameMaxLength)]
        [Comment("Name of Product")]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("Is Product validated")]
        public bool IsValidated { get; set; }

        [Required]
        [Range(CadConstants.PriceMin, CadConstants.PriceMax)]
        [Comment("Price of Product")]
        public decimal Price { get; set; }

        [Required]
        [Comment("3D Model of Product")]
        public int CadId { get; set; }

        [ForeignKey(nameof(CadId))]
        public Cad Cad { get; set; } = null!;

        [Required]
        [Comment("Category of Product")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = [];
    }
}
