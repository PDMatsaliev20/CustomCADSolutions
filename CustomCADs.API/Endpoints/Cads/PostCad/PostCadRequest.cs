namespace CustomCADs.API.Endpoints.Cads.PostCad
{
    public class PostCadRequest
    {
        public required IFormFile File { get; set; }
        public required IFormFile Image { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}
