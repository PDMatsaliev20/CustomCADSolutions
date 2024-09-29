using CustomCADs.API.Models.Cads;

namespace CustomCADs.API.Endpoints.Cads.PatchCad
{
    public class PatchCadRequest
    {
        public int Id { get; set; }
        public required string Type { get; set; }
        public required CoordinatesDTO Coordinates { get; set; }
    }
}
