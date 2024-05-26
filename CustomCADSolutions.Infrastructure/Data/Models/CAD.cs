using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using CustomCADSolutions.Infrastructure.Data.Models.Identity;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Cad
    {
        [Key]
        [Comment("Identification of 3D Model")]
        public int Id { get; set; }

        [Required]
        [Comment("Bytes of 3D Model")]
        public byte[] Bytes { get; set; } = null!;

        [Required]
        [MaxLength(CadConstants.XMax)]
        [Comment("X coordinate of 3D Model")]
        public int X { get; set; }
        
        [Required]
        [MaxLength(CadConstants.YMax)]
        [Comment("Y coordinate of 3D Model")]
        public int Y { get; set; }

        [Required]
        [MaxLength(CadConstants.ZMax)]
        [Comment("Z coordinate of 3D Model")]
        public int Z { get; set; }

        [Required]
        [Comment("CreationDate of 3D Model")]
        public DateTime CreationDate { get; set; }

        [Required]
        [Comment("Identification of the creator of the 3D Model")]
        public string CreatorId { get; set; } = null!;

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public AppUser Creator { get; set; } = null!;
    }
}
