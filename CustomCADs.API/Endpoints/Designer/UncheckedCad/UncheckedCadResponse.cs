using CustomCADs.API.Dtos;

namespace CustomCADs.API.Endpoints.Designer.UncheckedCad
{
    public class UncheckedCadResponse
    {
        public required int? PrevId { get; set; }
        public int Id { get; set; }
        public required string CadPath { get; set; }
        public CoordinatesDto CamCoordinates { get; set; } = new(0, 0, 0);
        public CoordinatesDto PanCoordinates { get; set; } = new(0, 0, 0);
        public required int? NextId { get; set; }
    }
}
