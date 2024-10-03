namespace CustomCADs.API.Endpoints.Categories.PutCategory
{
    public class PutCategoryRequest
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}
