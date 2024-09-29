using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Others;

namespace CustomCADs.API.Endpoints.Home.GalleryDetails
{
    public class GalleryDetailsResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CadPath { get; set; }
        public required string CreatorName { get; set; }
        public required string CreationDate { get; set; }
        public decimal Price { get; set; }
        public CoordinatesDTO CamCoordinates { get; set; } = new(0, 0, 0);
        public CoordinatesDTO PanCoordinates { get; set; } = new(0, 0, 0);
        public CategoryDTO Category { get; set; } = new();
    }
}
