using CustomCADs.API.Dtos;

namespace CustomCADs.API.Endpoints.Cads.RecentCads;

public class RecentCadsResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }
    public required string CreationDate { get; set; }
    public CategoryDto Category { get; set; } = new(0, string.Empty);
}
