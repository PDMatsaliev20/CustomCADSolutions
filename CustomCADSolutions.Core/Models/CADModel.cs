using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models.Identity;

namespace CustomCADSolutions.Core.Models
{
    public class CadModel
    {
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public byte[] Bytes { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int[] Coords { get; set; } = new int[3];

        [Required(ErrorMessage = RequiredErrorMessage)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public ProductModel Product { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string CreatorId { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public AppUser Creator { get; set; } = null!;
    }
}
