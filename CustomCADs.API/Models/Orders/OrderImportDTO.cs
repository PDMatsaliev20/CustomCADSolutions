using System.ComponentModel.DataAnnotations;
using static CustomCADs.Infrastructure.Data.DataConstants;
using static CustomCADs.Infrastructure.Data.DataConstants.OrderConstants;

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
        public int CategoryId { get; set; }
    }
}
