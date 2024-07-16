namespace CustomCADs.Core.Models
{
    public class CadQueryResult
    {
        public int Count { get; set; }
        public ICollection<CadModel> Cads { get; set; } = [];
    }
}
