using CustomCADSolutions.App.Resources.Shared;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models.Cads
{
    public class CadQueryInputModel
    {
        [Display(Name = nameof(SharedResources.Category),
            ResourceType = typeof(SharedResources))]
        public string? Category { get; set; }

        public string? Creator { get; set; }
        
        public bool? Validated { get; set; }
        
        public bool? Unvalidated { get; set; }

        [Display(Name = nameof(SharedResources.ByName),
            ResourceType = typeof(SharedResources))]
        public string? SearchName { get; set; }

        [Display(Name = nameof(SharedResources.ByCreator),
            ResourceType = typeof(SharedResources))]
        public string? SearchCreator { get; set; }

        [Display(Name = nameof(SharedResources.Sorting),
            ResourceType = typeof(SharedResources))]
        public CadSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int CadsPerPage { get; set; } = 4;
        
        public int Cols { get; set; } = 4;
        
        public int MaxCadsPerPage { get => Cols * (20 / Cols); } 

        public int TotalCadsCount { get; set; }

        public ICollection<CadViewModel> Cads { get; set; } = new List<CadViewModel>();

        public IEnumerable<string> Categories { get; set; } = Array.Empty<string>();
    }
}
