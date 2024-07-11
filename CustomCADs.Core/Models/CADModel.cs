using CustomCADs.Infrastructure.Data.Models;
using static CustomCADs.Infrastructure.Data.DataConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADs.Infrastructure.Data.Models.Identity;

namespace CustomCADs.Core.Models
{
    public class CadModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Extension { get; set; } = null!;

        [Required]
        public bool IsFolder { get; set; } = false;

        public string Path { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(CadConstants.NameMaxLength, MinimumLength = CadConstants.NameMinLength, ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        public bool IsValidated { get; set; }

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

        public Category Category { get; set; } = null!;

        public AppUser Creator { get; set; } = null!;

        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
