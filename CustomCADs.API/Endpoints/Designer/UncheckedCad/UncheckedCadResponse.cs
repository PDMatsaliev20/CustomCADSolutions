using CustomCADs.API.Models.Cads;

namespace CustomCADs.API.Endpoints.Designer.UncheckedCad
{
    public class UncheckedCadResponse
    {
        public required int? PrevId { get; set; }
        public required CadGetDTO Cad { get; set; }
        public required int? NextId { get; set; }
    }
}
