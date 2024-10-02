using CustomCADs.API.Dtos;

namespace CustomCADs.API.Endpoints.Home.GalleryDetails
{
    public class GalleryDetailsResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public required string CadPath { get; set; }
        public required string CreatorName { get; set; }
        public required string CreationDate { get; set; }
        public CoordinatesDto CamCoordinates { get; set; } = new(0, 0, 0);
        public CoordinatesDto PanCoordinates { get; set; } = new(0, 0, 0);
        public CategoryDto Category { get; set; } = new(0, string.Empty);
    }
}
