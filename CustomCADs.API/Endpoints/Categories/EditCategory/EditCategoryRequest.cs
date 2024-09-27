namespace CustomCADs.API.Endpoints.Categories.EditCategory
{
    public class EditCategoryRequest
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }
}
