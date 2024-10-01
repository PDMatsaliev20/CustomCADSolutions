using CustomCADs.API.Dtos;

namespace CustomCADs.API.Endpoints.Cads.GetCads
{
    public class GetCadsResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CreatorName { get; set; }
        public required string CreationDate { get; set; }
        public required string ImagePath { get; set; }
        public required string Status { get; set; }
        public CategoryDto Category { get; set; } = new(0, string.Empty);
    }
}
