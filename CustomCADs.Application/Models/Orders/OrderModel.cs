using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Orders.Enums;
using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.Orders.OrderConstants;
using static CustomCADs.Domain.Shared.SharedConstants;

namespace CustomCADs.Application.Models.Orders;

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

    public string ImagePath { get; set; } = null!;
    public string ImageExtension => '.' + ImagePath.Split('.')[^1].ToLower();

    public int? CadId { get; set; }
    public CadModel? Cad { get; set; }

    public string? DesignerId { get; set; }
    public UserModel? Designer { get; set; }

    [Required(ErrorMessage = RequiredErrorMessage)]
    public int CategoryId { get; set; }
    public CategoryModel Category { get; set; } = null!;

    [Required(ErrorMessage = RequiredErrorMessage)]
    public string BuyerId { get; set; } = null!;
    public UserModel Buyer { get; set; } = null!;
}
