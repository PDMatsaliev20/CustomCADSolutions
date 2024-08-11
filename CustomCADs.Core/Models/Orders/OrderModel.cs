using CustomCADs.Core.Models.Cads;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Entities.Enums;
using CustomCADs.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.OrderConstants;

namespace CustomCADs.Core.Models.Orders
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
        public bool ShouldBeDelivered { get; set; }

        public string? ImagePath { get; set; }
        public string? ImageExtension => '.' + ImagePath?.Split('.')[^1].ToLower();

        public int? CadId { get; set; }
        public CadModel? Cad { get; set; }

        public string? DesignerId { get; set; }
        public AppUser? Designer { get; set; }
        
        [Required(ErrorMessage = RequiredErrorMessage)]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public string BuyerId { get; set; } = null!;
        public AppUser Buyer { get; set; } = null!;
        
        public bool Validate(out IList<string> errors)
        {
            List<ValidationResult> validationResults = [];
            errors = [];

            if (!Validator.TryValidateObject(this, new(this), validationResults, true))
            {
                errors = validationResults.Select(result => result.ErrorMessage ?? string.Empty).ToList();
                return true;
            }

            return false;
        }
    }
}
