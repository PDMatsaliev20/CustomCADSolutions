namespace CustomCADs.API.Endpoints.Designer.PatchOrderStatus
{
    public class PatchOrderStatusRequest
    {
        public int Id { get; set; }
        public required string Status { get; set; }
        public int? CadId { get; set; }
    }
}
