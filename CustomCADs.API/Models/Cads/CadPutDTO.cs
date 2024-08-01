using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.CadConstants;
using static CustomCADs.Domain.DataConstants.CategoryConstants;

namespace CustomCADs.API.Models.Cads
{
    public class CadPutDTO
    {
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Range(IdMin, IdMax, ErrorMessage = RangeErrorMessage)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Range(PriceMin, PriceMax, ErrorMessage = RangeErrorMessage)]
        public decimal Price { get; set; }
    }
}
