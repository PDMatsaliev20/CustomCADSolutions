namespace CustomCADs.API.Models.Queries
{
    public class CadQueryResultDTO
    {
        public int Count { get; set; }
        public ICollection<Cads.CadGetDTO> Cads { get; set; } = [];
    }
}
