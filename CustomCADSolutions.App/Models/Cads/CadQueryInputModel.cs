using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models.Cads
{
    public class CadQueryInputModel
    {
        public string? Category { get; set; }
        
        public string? Creator { get; set; }
        
        public bool? Validated { get; set; }
        
        public bool? Unvalidated { get; set; }

        [Display(Name = "Name, Category or Creator")]
        public string? SearchTerm { get; set; }

        public CadSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalCadsCount { get; set; }

        public ICollection<CadViewModel> Cads { get; set; } = new List<CadViewModel>();

        public IEnumerable<string> Categories { get; set; } = Array.Empty<string>();
    }
}
