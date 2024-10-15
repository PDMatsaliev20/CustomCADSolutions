using CustomCADs.API.Dtos;

namespace CustomCADs.API.Endpoints.Designer.OngoingOrders;

public class OngoingOrdersResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ImagePath { get; set; }
    public required string OrderDate { get; set; }
    public bool ShouldBeDelivered { get; set; }
    public CategoryDto Category { get; set; } = new(0, string.Empty);
}
