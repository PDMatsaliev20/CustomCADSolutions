using CustomCADs.API.Dtos;

namespace CustomCADs.API.Endpoints.Orders.RecentOrders;

public class RecentOrdersResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Status { get; set; }
    public required string OrderDate { get; set; }
    public CategoryDto Category { get; set; } = new(0, string.Empty);
}
