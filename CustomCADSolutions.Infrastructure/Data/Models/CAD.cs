using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CustomCADSolutions.Infrastructure.Constants;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Cad
    {
        [Key]
        [Comment("Identification of 3D Model")]
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.Cad.NameMaxLength)]
        [Comment("Name of 3D Model")]
        public string Name { get; set; } = null!;

        [Comment("CreationDate of 3D Model")]
        public DateTime? CreationDate { get; set; }

        [Comment("Url of 3D Model")]
        public string? Url { get; set; }
        
        [Required]
        [Comment("Category of 3D Model")]
        public Category Category { get; set; }

        public Order? Order { get; set; }
    }
}
