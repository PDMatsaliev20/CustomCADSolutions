using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using CustomCADSolutions.Infrastructure.Data.Models;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using static CustomCADSolutions.Infrastructure.Data.DataConstants.OrderConstants;

namespace CustomCADSolutions.Core.Models
{
    public class OrderModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, 
            ErrorMessage = LengthErrorMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public OrderStatus Status { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public bool ShouldShow { get; set; } = true;

        public int? CadId { get; set; }
        
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string BuyerId { get; set; } = null!;

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [ForeignKey(nameof(CadId))]
        public CadModel? Cad { get; set; }
            
        [ForeignKey(nameof(BuyerId))]
        public AppUser Buyer { get; set; } = null!;
    }
}
