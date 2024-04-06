using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Cad
    {
        [Key]
        [Comment("Identification of 3D Model")]
        public int Id { get; set; }

        [Comment("Bytes of 3D Model")]
        public byte[]? Bytes { get; set; }

        [Required]
        [MaxLength(CadConstants.NameMaxLength)]
        [Comment("Name of 3D Model")]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("Is 3D Model validated")]
        public bool IsValidated { get; set; }

        [Comment("CreationDate of 3D Model")]
        public DateTime? CreationDate { get; set; }

        [MaxLength(CadConstants.XMax)]
        [Comment("X coordinate of 3D Model")]
        public int X { get; set; }
        
        [MaxLength(CadConstants.YMax)]
        [Comment("Y coordinate of 3D Model")]
        public int Y { get; set; }

        [MaxLength(CadConstants.ZMax)]
        [Comment("Z coordinate of 3D Model")]
        public int Z { get; set; }

        [Comment("Rgb value of 3D Model")]
        public int R { get; set; }

        [Comment("rGb value of 3D Model")]
        public int G { get; set; }

        [Comment("rgB value of 3D Model")]
        public int B { get; set; }

        [RegularExpression(CadConstants.SpinAxisRegEx)]
        [Comment("Spin axis of 3D Model")]
        public char? SpinAxis { get; set; }

        [Comment("Identification of the creator of the 3D Model")]
        public string? CreatorId { get; set; }

        [Required]
        [Comment("Category of 3D Model")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public AppUser? Creator { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
