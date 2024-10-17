using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Enums;
using CustomCADs.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.Application.Models.Cads;

public class CadModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = RequiredErrorMessage)]
    [StringLength(CadConstants.NameMaxLength, MinimumLength = CadConstants.NameMinLength, ErrorMessage = LengthErrorMessage)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = RequiredErrorMessage)]
    [StringLength(CadConstants.DescriptionMaxLength, MinimumLength = CadConstants.DescriptionMinLength, ErrorMessage = LengthErrorMessage)]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = RequiredErrorMessage)]
    public CadStatus Status { get; set; }

    [Required(ErrorMessage = RequiredErrorMessage)]
    [Range(typeof(decimal), CadConstants.PriceMinString, CadConstants.PriceMaxString)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = RequiredErrorMessage)]
    public DateTime CreationDate { get; set; }

    [Required(ErrorMessage = RequiredErrorMessage)]
    public Coordinates CamCoordinates { get; set; } = new(0, 0, 0);
    
    [Required(ErrorMessage = RequiredErrorMessage)]
    public Coordinates PanCoordinates { get; set; } = new(0, 0, 0);

    [Required(ErrorMessage = RequiredErrorMessage)]
    public Paths Paths { get; set; } = new();

    [Required(ErrorMessage = RequiredErrorMessage)]
    [AllowedValues(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
        ErrorMessage = "Existing Categories have IDs: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]")]
    public int CategoryId { get; set; }
    public CategoryModel Category { get; set; } = null!;

    [Required(ErrorMessage = RequiredErrorMessage)]
    public string CreatorId { get; set; } = null!;
    public UserModel Creator { get; set; } = null!;

    public ICollection<OrderModel> Orders { get; set; } = [];
}
