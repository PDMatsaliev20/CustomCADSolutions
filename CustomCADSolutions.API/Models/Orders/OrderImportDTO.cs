using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using static CustomCADSolutions.Infrastructure.Data.DataConstants.OrderConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Core.Mappings.CadDTOs;
using CustomCADSolutions.API.Models.CadDTOs;

namespace CustomCADSolutions.API.Models.Orders
{
    public class OrderImportDTO
    {
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string Status { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public bool ShouldShow { get; set; }

        public int? CadId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string BuyerId { get; set; } = null!;

        public CadImportDTO? Cad { get; set; }
    }
}
