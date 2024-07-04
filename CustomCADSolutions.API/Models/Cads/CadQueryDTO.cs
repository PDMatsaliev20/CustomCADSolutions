using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.API.Models.Cads
{
    public class CadQueryDTO
    {
        public int Count { get; set; }
        public ICollection<CadExportDTO> Cads { get; set; } = [];
    }
}
