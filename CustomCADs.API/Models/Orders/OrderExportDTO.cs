using CustomCADs.API.Models.Others;

namespace CustomCADs.API.Models.Orders
{
    public class OrderExportDTO
    {
        public int Id { get; set; } 
        public bool ShouldBeDelivered { get; set; } 
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string OrderDate { get; set; } = null!;
        public string? ImagePath { get; set; } 
        public string? DesignerName { get; set; }
        public string? DesignerEmail { get; set; }
        public string BuyerName { get; set; } = null!;
        public int? CadId { get; set; }
        public CategoryDTO Category { get; set; } = null!;
    }
}
