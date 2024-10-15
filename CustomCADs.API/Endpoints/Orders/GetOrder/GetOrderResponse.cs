using CustomCADs.API.Dtos;

namespace CustomCADs.API.Endpoints.Orders.GetOrder
{
    public class GetOrderResponse
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required bool ShouldBeDelivered { get; set; }
        public required string Status { get; set; }
        public required string OrderDate { get; set; }
        public required string BuyerName { get; set; }
        public CategoryDto Category { get; set; } = new(0, string.Empty);
    }
}
