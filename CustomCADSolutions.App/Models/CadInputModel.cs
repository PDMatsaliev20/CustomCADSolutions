using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models;

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
        public int CategoryId { get; set; } 

        [Required(ErrorMessage = RequiredErrorMessage)]
        [Display(Name = CadConstants.FileDisplay)]
        public IFormFile CadFile { get; set; } = null!;
        
        [Range(CadConstants.XMin, CadConstants.XMax, ErrorMessage = RangeErrorMessage)]
        public short X { get; set; }
        
        [Range(CadConstants.YMin, CadConstants.YMax, ErrorMessage = RangeErrorMessage)]
        public short Y { get; set; }

        [Range(CadConstants.ZMin, CadConstants.ZMax, ErrorMessage = RangeErrorMessage)]
        public short Z { get; set; }

        [Range(0, CadConstants.SpinFactorMax * 100, ErrorMessage = RangeErrorMessage)]
        [Display(Name = "Speed")]
        public int SpinFactor { get; set; }

        [RegularExpression("[xyz]", ErrorMessage = CadConstants.SpinAxisErrorMessage)]
        [Display(Name = "Axis of spin")]
        public char? SpinAxis { get; set; }

        public Category[] Categories { get; set; } = null!;
    }
}
