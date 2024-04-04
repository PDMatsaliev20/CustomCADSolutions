using System.Text.Json.Serialization;

namespace CustomCADSolutions.App.Mappings.CadDTOs
{
    public class CadQueryDTO
    {
        [JsonPropertyName("totalCadsCount")]
        public int TotalCount { get; set;}

        [JsonPropertyName("cads")]
        public ICollection<CadExportDTO> Cads { get; set; } = Array.Empty<CadExportDTO>();
    }
}
