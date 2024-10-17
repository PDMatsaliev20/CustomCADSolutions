using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Cads.Enums;
using CustomCADs.Domain.Cads.ValueObjects;
using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.Cads.CadConstants;
using static CustomCADs.Domain.Shared.SharedConstants;

namespace CustomCADs.Application.Models.Cads;

public class CadModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = RequiredErrorMessage)]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = LengthErrorMessage)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = RequiredErrorMessage)]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = LengthErrorMessage)]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = RequiredErrorMessage)]
    public CadStatus Status { get; set; }

    [Required(ErrorMessage = RequiredErrorMessage)]
    [Range(typeof(decimal), PriceMinString, PriceMaxString)]
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
    public int CategoryId { get; set; }
    public CategoryModel Category { get; set; } = null!;

    [Required(ErrorMessage = RequiredErrorMessage)]
    public string CreatorId { get; set; } = null!;
    public UserModel Creator { get; set; } = null!;
}
