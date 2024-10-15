namespace CustomCADs.API.Endpoints.Cads.PutCad;

public class PutCadRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int CategoryId { get; set; }
    public required decimal Price { get; set; }
    public IFormFile? Image { get; set; }
}
