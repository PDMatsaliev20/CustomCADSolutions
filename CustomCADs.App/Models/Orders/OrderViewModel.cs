namespace CustomCADs.App.Models.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string OrderDate { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? CadId { get; set; }
        public string? DesignerName { get; set; }
        public string BuyerId { get; set; } = null!;
        public string BuyerName { get; set; } = null!;
    }
}
