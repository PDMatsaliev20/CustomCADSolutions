using CustomCADs.Domain.Entities.Enums;

namespace CustomCADs.Core.Models.Orders
{
    public class OrderQuery
    {
        public string? Buyer { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
