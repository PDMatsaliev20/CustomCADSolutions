using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Core.Models
{
    public class CadModel
    {
        [Key]
        public int Id { get; set; }

        public byte[]? CadInBytes { get; set; } 

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(Cad.NameMaxLength, MinimumLength = Cad.NameMinLength, ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        public DateTime? CreationDate { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public Category Category { get; set; }

        public int? OrderId { get; set; }

        public OrderModel? Order { get; set; }
    }
}
