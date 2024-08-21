namespace CustomCADs.Application.Models
{
    public class SearchModel
    {
        public string Sorting { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Owner { get; set; }
    }
}
