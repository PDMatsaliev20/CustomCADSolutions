namespace CustomCADs.API.Models.Orders
{
    public class OrderExportDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string OrderDate { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string BuyerName { get; set; } = null!;
        public int? CadId { get; set; }
    }
}
