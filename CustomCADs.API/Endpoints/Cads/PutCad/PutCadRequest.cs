using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.CadConstants;

namespace CustomCADs.API.Endpoints.Cads.PutCad
{
    public class PutCadRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public required string Name { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,
            ErrorMessage = LengthErrorMessage)]
        public required string Description { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [AllowedValues(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
            ErrorMessage = "Existing Categories have IDs: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]")]
        public required int CategoryId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Range(PriceMin, PriceMax, ErrorMessage = RangeErrorMessage)]
        public required decimal Price { get; set; }

        public IFormFile? Image { get; set; }
    }
}
