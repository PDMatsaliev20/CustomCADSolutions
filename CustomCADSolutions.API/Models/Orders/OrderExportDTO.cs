namespace CustomCADSolutions.API.Models.Orders
{
    public class OrderExportDTO
    {
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string OrderDate { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string BuyerName { get; set; } = null!;
        public int? CadId { get; set; }
    }
}
