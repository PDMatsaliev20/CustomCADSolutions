namespace CustomCADs.API.Dtos
{
    public class CadResultDto<TCad> where TCad : class
    {
        public int Count { get; set; }
        public ICollection<TCad> Cads { get; set; } = [];
    }
}
