using CustomCADs.Domain.Cads.Enums;
using CustomCADs.Domain.Cads.ValueObjects;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Users;

namespace CustomCADs.Domain.Cads;

public class Cad
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public CadStatus Status { get; set; }
    public decimal Price { get; set; }
    public DateTime CreationDate { get; set; }
    public Coordinates CamCoordinates { get; set; } = new();
    public Coordinates PanCoordinates { get; set; } = new();
    public Paths Paths { get; set; } = new();
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public required string CreatorId { get; set; }
    public User Creator { get; set; } = null!;
}
