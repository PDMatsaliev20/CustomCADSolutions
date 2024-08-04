namespace CustomCADs.Core.Models
{
    public class SearchModel
    {
        public string Sorting { get; set; } = Enum.GetName(Domain.Entities.Enums.Sorting.Newest)!;
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Owner { get; set; }
    }
}
