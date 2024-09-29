namespace CustomCADs.API.Endpoints.Orders.PutOrder
{
    public class PutOrderRequest
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
