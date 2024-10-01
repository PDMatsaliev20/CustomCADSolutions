using CustomCADs.API.Dtos;

namespace CustomCADs.API.Endpoints.Home.MainCad
{
    public class MainCadResponse
    {
        public required string CadPath { get; set; }
        public required CoordinatesDto CamCoordinates { get; set; }
        public required CoordinatesDto PanCoordinates { get; set; }
    }
}
