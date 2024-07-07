using CustomCADSolutions.API.Models.Others;

namespace CustomCADSolutions.API.Models.Queries
{
    public class CadQueryDTO
    {
        public string? Category { get; set; }
        public string? Creator { get; set; }
        public string? SearchName { get; set; }
        public string? SearchCreator { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int CadsPerPage { get; set; } = 3;
        public bool Validated { get; set; } = true;
        public bool Unvalidated { get; set; } = true;
        public CadSortingDTO Sorting { get; set; } = null!;
    }
}
