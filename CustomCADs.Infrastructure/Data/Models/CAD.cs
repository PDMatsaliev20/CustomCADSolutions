using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static CustomCADs.Infrastructure.Data.DataConstants;
using CustomCADs.Infrastructure.Data.Models.Identity;

namespace CustomCADs.Infrastructure.Data.Models
{
    public class Cad
    {
        [Key]
        [Comment("Identification of 3D Model")]
        public int Id { get; set; }

        [Required]
        [MaxLength(CadConstants.NameMaxLength)]
        [Comment("Name of 3D Model")]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("Extension name of 3D Model")]
        public string Extension { get; set; } = null!;

        [Required]
        public bool IsFolder { get; set; }

        [Comment("Path to 3D Model")]
        public string? Path { get; set; } 
        
        [Required]
        [Comment("Is 3D Model validated")]
        public bool IsValidated { get; set; }

        [Required]
        [Range(CadConstants.PriceMin, CadConstants.PriceMax)]
        [Comment("Price of 3d model")]
        public decimal Price { get; set; }

        [Required]
        [Comment("CreationDate of 3D Model")]
        public DateTime CreationDate { get; set; }

        [Required]
        [Range(CadConstants.CoordMax, CadConstants.CoordMin)]
        [Comment("Camera's X coordinate of 3D Model")]
        public int X { get; set; }
        
        [Required]
        [Range(CadConstants.CoordMax, CadConstants.CoordMin)]
        [Comment("Camera's Y coordinate of 3D Model")]
        public int Y { get; set; }

        [Required]
        [Range(CadConstants.CoordMax, CadConstants.CoordMin)]
        [Comment("Camera's Z coordinate of 3D Model")]
        public int Z { get; set; }

        [Required]
        [Range(CadConstants.PanMin, CadConstants.PanMax)]
        [Comment("Panning along the x-axis of 3D Model")]
        public int PanX { get; set; }
        
        [Required]
        [Range(CadConstants.PanMin, CadConstants.PanMax)]
        [Comment("Panning along the y-axis of 3D Model")]
        public int PanY { get; set; }
        
        [Required]
        [Range(CadConstants.PanMin, CadConstants.PanMax)]
        [Comment("Panning along the z-axis of 3D Model")]
        public int PanZ { get; set; }

        [Required]
        [Comment("Identification of the creator of the 3D Model")]
        public string CreatorId { get; set; } = null!;

        [Required]
        [Comment("Category of 3D Model")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public AppUser Creator { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
