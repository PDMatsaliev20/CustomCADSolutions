using CustomCADs.Domain.Enums;
using CustomCADs.Domain.ValueObjects;
using CustomCADs.Infrastructure.Data.Identity;
using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.Infrastructure.Data.Entities
{
    public class PCad 
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CadConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(CadConstants.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public CadStatus Status { get; set; }

        [Required]
        [Range(CadConstants.PriceMin, CadConstants.PriceMax)]
        public decimal Price { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public Coordinates CamCoordinates { get; set; } = null!;

        [Required]
        public Coordinates PanCoordinates { get; set; } = null!;

        [Required]
        public Paths Paths { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; } 
        public PCategory Category { get; set; } = null!;

        [Required]
        public string CreatorId { get; set; } = null!;
        public AppUser Creator { get; set; } = null!;

        public ICollection<POrder> Orders { get; set; } = [];
    }
}
