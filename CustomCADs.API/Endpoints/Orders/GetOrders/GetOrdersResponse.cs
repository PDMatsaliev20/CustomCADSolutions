using CustomCADs.API.Dtos;

namespace CustomCADs.API.Endpoints.Orders.GetOrders;

public class GetOrdersResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ImagePath { get; set; }
    public required string OrderDate { get; set; }
    public string? DesignerEmail { get; set; }
    public string? DesignerName { get; set; }
    public CategoryDto Category { get; set; } = new(0, string.Empty);
}
