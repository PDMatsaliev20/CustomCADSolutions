using CustomCADs.API.Models.Cads;

namespace CustomCADs.API.Models.Queries
{
    public class CadQueryResultDTO
    {
        public int Count { get; set; }
        public ICollection<CadExportDTO> Cads { get; set; } = [];
    }
}
