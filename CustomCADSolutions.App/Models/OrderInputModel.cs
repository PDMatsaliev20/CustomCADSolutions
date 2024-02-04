using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models
{
    public class OrderInputModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = LengthErrorMessage)]
        [Display(Name = Order.NameDisplay)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Display(Name = Order.CategoryDisplay)]
        public int Category { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(Order.DescriptionMaxLength, MinimumLength = Order.DescriptionMinLength, ErrorMessage = LengthErrorMessage)]
        [Display(Name = Order.DescriptionDisplay)]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }
    }
}
