using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomCADSolutions.Core.Models
{
    public class CadModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(CadConstants.NameMaxLength, MinimumLength = CadConstants.NameMinLength, ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        public bool Validated { get; set; }

        public DateTime? CreationDate { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public Category Category { get; set; }
        
        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
        
        public (short, short, short) Coords { get; set; }

        [Range(CadConstants.SpinFactorMin, CadConstants.SpinFactorMax)]
        public double SpinFactor { get; set; }

        [RegularExpression("[xyz]", ErrorMessage = CadConstants.SpinAxisErrorMessage)]
        public char? SpinAxis { get; set; }

        public string? CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public IdentityUser? Creator { get; set; }
    }
}
