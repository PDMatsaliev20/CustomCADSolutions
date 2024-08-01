namespace CustomCADs.Core.Models.Cads
{
    public class CadQueryResult
    {
        public int Count { get; set; }
        public ICollection<CadModel> Cads { get; set; } = [];
    }
}
