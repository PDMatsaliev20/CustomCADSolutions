using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.OrderConstants;

namespace CustomCADs.API.Models.Orders
{
    public class OrderImportDTO
    {
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [AllowedValues(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
            ErrorMessage = "Existing Categories have IDs: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public bool ShouldBeDelivered { get; set; } = false;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public IFormFile Image { get; set; } = null!;
    }
}
