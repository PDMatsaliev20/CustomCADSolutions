namespace CustomCADs.API.Endpoints.Designer.PatchCadStatus
{
    public class PatchCadStatusRequest
    {
        public int Id { get; set; }
        public required string Status { get; set; }
    }
}
