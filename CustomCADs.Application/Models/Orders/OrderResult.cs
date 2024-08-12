namespace CustomCADs.Core.Models.Orders
{
    public class OrderResult
    {
        public int Count { get; set; }
        public ICollection<OrderModel> Orders { get; set; } = [];
    }
}
