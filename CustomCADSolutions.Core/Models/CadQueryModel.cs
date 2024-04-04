using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.Core.Models
{
    public class CadQueryModel
    {
        public int TotalCount { get; set; }
        public ICollection<CadModel> Cads { get; set; } = new List<CadModel>();

        public string? Category { get; set; }
        public string? Creator { get; set; }
        public string? LikeName { get; set; }
        public string? LikeCreator { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int CadsPerPage { get; set; } = 1;
        public bool Validated { get; set; }
        public bool Unvalidated { get; set; } 
        public CadSorting Sorting { get; set; }
    }
}