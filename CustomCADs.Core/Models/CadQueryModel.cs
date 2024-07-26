using CustomCADs.Domain.Entities.Enums;

namespace CustomCADs.Core.Models
{
    public class CadQueryModel
    {
        public string? Category { get; set; }
        public string? Creator { get; set; }
        public string? SearchName { get; set; }
        public string? SearchCreator { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int CadsPerPage { get; set; } = 3;
        public CadStatus? Status { get; set; }
        public CadSorting Sorting { get; set; }
    }
}