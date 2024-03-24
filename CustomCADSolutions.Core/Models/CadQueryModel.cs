namespace CustomCADSolutions.Core.Models
{
    public class CadQueryModel
    {
        public int TotalCount { get; set; }
        public ICollection<CadModel> CadModels { get; set; } = new List<CadModel>();
    }
}