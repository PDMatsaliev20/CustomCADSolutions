using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.App.Areas.Bg.Models
{
    public class OrderInputModel
    {
        public int CadId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(CadConstants.NameMaxLength, 
            MinimumLength = CadConstants.NameMinLength, 
            ErrorMessage = LengthErrorMessage)]
        [Display(Name = "Име на 3D модела")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Display(Name = "Категория на 3D модела")]
        public Category Category { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(OrderConstants.DescriptionMaxLength, 
            MinimumLength = OrderConstants.DescriptionMinLength, 
            ErrorMessage = LengthErrorMessage)]
        [Display(Name = "Описание на 3D модела")]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }
        
        public Infrastructure.Data.Models.Enums.OrderStatus Status { get; set; }
    }
}
