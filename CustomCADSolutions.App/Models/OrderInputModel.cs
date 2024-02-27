using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.App.Models
{
    public class OrderInputModel
    {
        public int CadId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(CadConstants.NameMaxLength, 
            MinimumLength = CadConstants.NameMinLength, 
            ErrorMessage = LengthErrorMessage)]
        [Display(Name = OrderConstants.NameDisplay)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Display(Name = OrderConstants.CategoryDisplay)]
        public int CategoryId { get; set; } 

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(OrderConstants.DescriptionMaxLength, 
            MinimumLength = OrderConstants.DescriptionMinLength, 
            ErrorMessage = LengthErrorMessage)]
        [Display(Name = OrderConstants.DescriptionDisplay)]
        public string Description { get; set; } = null!;

        public DateTime OrderDate { get; set; }
        
        public OrderStatus Status { get; set; }

        public Category[]? Categories { get; set; }
    }
}
