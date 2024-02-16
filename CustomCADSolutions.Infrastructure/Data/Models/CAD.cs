using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CustomCADSolutions.Infrastructure.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Cad
    {
        [Key]
        [Comment("Identification of 3D Model")]
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.CadConstants.NameMaxLength)]
        [Comment("Name of 3D Model")]
        public string Name { get; set; } = null!;

        [Comment("CreationDate of 3D Model")]
        public DateTime? CreationDate { get; set; }
        
        [Required]
        [Comment("Category of 3D Model")]
        public Category Category { get; set; }

        [Comment("X coordinate of 3D Model")]
        public short X { get; set; }
        
        [Comment("Y coordinate of 3D Model")]
        public short Y { get; set; }
        
        [Comment("Z coordinate of 3D Model")]
        public short Z { get; set; }

        [Comment("Spin axis of 3D Model")]
        public char? SpinAxis { get; set; }
        
        [Comment("Spinning constant of 3D Model")]
        public double SpinFactor { get; set; }

        [Comment("Identification of the creator of the 3D Model")]
        public string? CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public IdentityUser? Creator { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
