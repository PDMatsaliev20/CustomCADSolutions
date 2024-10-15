using CustomCADs.API.Dtos;

namespace CustomCADs.API.Endpoints.Cads.PostCad;

public class PostCadResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string CreatorName { get; set; }
    public required string CreationDate { get; set; }
    public decimal Price { get; set; }
    public required string CadPath { get; set; }
    public required string ImagePath { get; set; }
    public CoordinatesDto CamCoordinates { get; set; } = new(0, 0, 0);
    public CoordinatesDto PanCoordinates { get; set; } = new(0, 0, 0);
    public required string Status { get; set; }
    public CategoryDto Category { get; set; } = new(0, string.Empty);
}
