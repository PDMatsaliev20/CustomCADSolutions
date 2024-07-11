namespace CustomCADs.API.Models.Queries
{
    public class CadQueryResultDTO
    {
        public int Count { get; set; }
        public ICollection<Cads.CadExportDTO> Cads { get; set; } = [];
    }
}
