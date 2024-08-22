using CustomCADs.Domain.Enums;
using CustomCADs.Domain.Identity;
using CustomCADs.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.Domain.Entities
{
    public class Cad : IEntity<int>
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
        public Category Category { get; set; } = null!;

        [Required]
        public string CreatorId { get; set; } = null!;
        public AppUser Creator { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = [];
    }
}
