namespace CustomCADs.Application.Models.Cads
{
    public class CadResult
    {
        public int Count { get; set; }
        public ICollection<CadModel> Cads { get; set; } = [];
    }
}
