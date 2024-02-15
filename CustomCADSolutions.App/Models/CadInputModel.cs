using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models
{
    public class CadInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(CadConstants.NameMaxLength, 
            MinimumLength = CadConstants.NameMinLength, 
            ErrorMessage = LengthErrorMessage)]
        [Display(Name = CadConstants.NameDisplay)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Display(Name = CadConstants.CategoryDisplay)]
        public Category Category { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Display(Name = CadConstants.FileDisplay)]
        public IFormFile CadFile { get; set; } = null!;
        
        [Required(ErrorMessage = RequiredErrorMessage)]
        [Display(Name = CadConstants.XDisplay)]
        public short X { get; set; }
        
        [Required(ErrorMessage = RequiredErrorMessage)]
        [Display(Name = CadConstants.YDisplay)]
        public short Y { get; set; }
        
        [Required(ErrorMessage = RequiredErrorMessage)]
        [Display(Name = CadConstants.ZDisplay)]
        public short Z { get; set; }
    }
}
