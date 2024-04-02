using CustomCADSolutions.Infrastructure.Data.Models;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Drawing;

namespace CustomCADSolutions.Core.Models
{
    public class CadModel
    {
        [Key]
        public int Id { get; set; }

        public byte[]? Bytes { get; set; }

        public Color Color { get; set; } = Color.FromArgb(0, CadConstants.R, CadConstants.G, CadConstants.B);

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(CadConstants.NameMaxLength, MinimumLength = CadConstants.NameMinLength, ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        public bool IsValidated { get; set; }

        public DateTime? CreationDate { get; set; }
        
        public (int, int, int) Coords { get; set; }

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
