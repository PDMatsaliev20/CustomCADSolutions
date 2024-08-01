using CustomCADs.Domain.Entities.Enums;

namespace CustomCADs.Core.Models.Orders
{
    public class OrderSearch
    {
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Buyer { get; set; }
        public string Sorting { get; set; }
    }
}
