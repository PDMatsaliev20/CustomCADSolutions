namespace CustomCADs.API.Models.Queries
{
    public class OrderResultDTO
    {
        public int Count { get; set; }
        public ICollection<Orders.OrderExportDTO> Orders { get; set; } = [];
    }
}
