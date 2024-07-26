using CustomCADs.Domain.Entities.Enums;
using CustomCADs.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.Core.Models
{
    public class CadModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string CadExtension { get; set; } = null!;
        
        [Required]
        public string ImageExtension { get; set; } = null!;

        [Required]
        public bool IsFolder { get; set; } = false;

        public string ImagePath { get; set; } = null!;
        
        public string CadPath { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(CadConstants.NameMaxLength, MinimumLength = CadConstants.NameMinLength, ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;
        
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(CadConstants.DescriptionMaxLength, MinimumLength = CadConstants.DescriptionMinLength, ErrorMessage = LengthErrorMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public CadStatus Status { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Range(CadConstants.PriceMin, CadConstants.PriceMax)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int[] Coords { get; set; } = new int[3];

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int[] PanCoords { get; set; } = new int[3];

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string CreatorId { get; set; } = null!;

        public CategoryModel Category { get; set; } = null!;

        public AppUser Creator { get; set; } = null!;

        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
