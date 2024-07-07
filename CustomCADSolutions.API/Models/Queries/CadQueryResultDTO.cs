using CustomCADSolutions.API.Models.Cads;

namespace CustomCADSolutions.API.Models.Queries
{
    public class CadQueryResultDTO
    {
        public int Count { get; set; }
        public ICollection<CadExportDTO> Cads { get; set; } = [];
    }
}
