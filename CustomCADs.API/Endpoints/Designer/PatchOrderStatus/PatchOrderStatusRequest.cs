namespace CustomCADs.API.Endpoints.Designer.PatchOrderStatus;

public class PatchOrderStatusRequest
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public int? CadId { get; set; }
}
