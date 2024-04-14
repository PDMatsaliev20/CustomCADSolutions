using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using static CustomCADSolutions.Infrastructure.Data.DataConstants.CadConstants;

namespace CustomCADSolutions.Core.Mappings.CadDTOs
{
    public class CadImportDTO
    {
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, 
            ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public bool IsValidated { get; set; }

        [RegularExpression(SpinAxisRegEx, ErrorMessage = SpinAxisErrorMessage)]
        public char? SpinAxis { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = RequiredErrorMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string CreatorId { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int[] Coords { get; set; } = new int[3];

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int[] RGB { get; set; } = new int[3] { 255, 255, 255 };
    }
}