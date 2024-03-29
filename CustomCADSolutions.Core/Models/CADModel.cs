using CustomCADSolutions.Infrastructure.Data.Models;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CustomCADSolutions.Core.Models
{
    public class CadModel
    {
        [Key]
        public int Id { get; set; }

        public byte[]? Bytes { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(CadConstants.NameMaxLength, MinimumLength = CadConstants.NameMinLength, ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        public bool IsValidated { get; set; }

        public DateTime? CreationDate { get; set; }
        
        public (short, short, short) Coords { get; set; }

        [RegularExpression(CadConstants.SpinAxisRegEx, ErrorMessage = CadConstants.SpinAxisErrorMessage)]
        public char? SpinAxis { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; set; }

        public string? CreatorId { get; set; }

        public Category Category { get; set; } = null!;

        public IdentityUser? Creator { get; set; }
        
        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
