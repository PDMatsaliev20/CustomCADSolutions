namespace CustomCADSolutions.App.Models.Orders
{
    public class OrderViewModel
    {
        public int CadId { get; set; }

        public string BuyerId { get; set; } = null!;
        
        public string Name { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string BgCategory { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string OrderDate { get; set; } = null!;

        public string Status { get; set; } = null!;
        
        public string BuyerName { get; set; } = null!;
    }
}
