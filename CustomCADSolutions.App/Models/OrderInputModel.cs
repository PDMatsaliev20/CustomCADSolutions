using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models
{
    public class OrderInputModel
    {
        public int CadId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = LengthErrorMessage)]
        [Display(Name = OrderConstants.NameDisplay)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Display(Name = OrderConstants.CategoryDisplay)]
        public int Category { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(OrderConstants.DescriptionMaxLength, MinimumLength = OrderConstants.DescriptionMinLength, ErrorMessage = LengthErrorMessage)]
        [Display(Name = OrderConstants.DescriptionDisplay)]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }
    }
}
