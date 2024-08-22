using CustomCADs.Domain.Enums;

namespace CustomCADs.Application.Models.Orders
{
    public class OrderQuery
    {
        public string? Buyer { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
