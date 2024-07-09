using System.ComponentModel.DataAnnotations;
using static CustomCADs.Infrastructure.Data.DataConstants;
using static CustomCADs.Infrastructure.Data.DataConstants.CadConstants;

namespace CustomCADs.API.Models.Cads
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

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string CreatorId { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int[] Coords { get; set; } = new int[3];
        
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int[] PanCoords { get; set; } = new int[3];
    }
}