using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CustomCADSolutions.Infrastructure.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Cad
    {
        [Key]
        [Comment("Identification of 3D Model")]
        public int Id { get; set; }
        
        [Comment("Byte Array representing 3D Model")]
        public byte[]? CadInBytes { get; set; } 

        [Required]
        [MaxLength(DataConstants.Cad.NameMaxLength)]
        [Comment("Name of 3D Model")]
        public string Name { get; set; } = null!;

        [Comment("CreationDate of 3D Model")]
        public DateTime? CreationDate { get; set; }
        
        [Required]
        [Comment("Category of 3D Model")]
        public Category Category { get; set; }

        public int? OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
    }
}
