namespace CustomCADs.Core.Models
{
    public class SearchModel
    {
        public string Sorting { get; set; } = null!;
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Owner { get; set; }
    }
}
