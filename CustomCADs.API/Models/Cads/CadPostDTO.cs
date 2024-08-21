using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.CadConstants;

namespace CustomCADs.API.Models.Cads
{
    public class CadPostDTO
    {
        [Required(ErrorMessage = RequiredErrorMessage)]
        public IFormFile File { get; set; } = null!;
        
        [Required(ErrorMessage = RequiredErrorMessage)]
        public IFormFile Image { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [AllowedValues(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
            ErrorMessage = "Existing Categories have IDs: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Range(PriceMin, PriceMax, ErrorMessage = RangeErrorMessage)]
        public decimal Price { get; set; }
    }
}
