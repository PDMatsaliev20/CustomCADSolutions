namespace CustomCADSolutions.Core.Models
{
    public class CadQueryResult
    {
        public int TotalCount { get; set; }
        public ICollection<CadModel> Cads { get; set; } = new List<CadModel>();
    }
}
