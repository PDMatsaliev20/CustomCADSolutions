using CustomCADs.App.Resources.Shared;
using CustomCADs.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomCADs.App.Models.Cads.View
{
    public class CadQueryInputModel
    {
        [Display(Name = nameof(DisplayResources.Category), ResourceType = typeof(DisplayResources))]
        public string? Category { get; set; }

        public string? Creator { get; set; }

        [Display(Name = nameof(DisplayResources.ByName), ResourceType = typeof(DisplayResources))]
        public string? SearchName { get; set; }

        [Display(Name = nameof(DisplayResources.ByCreator), ResourceType = typeof(DisplayResources))]
        public string? SearchCreator { get; set; }

        [Display(Name = nameof(DisplayResources.Sorting), ResourceType = typeof(DisplayResources))]
        public Sorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int CadsPerPage { get; set; } = 3;

        public int Cols { get; set; } = 3;

        public int MaxCadsPerPage { get => Cols * (20 / Cols); }

        public int TotalCount { get; set; }

        public ICollection<CadViewModel> Cads { get; set; } = new List<CadViewModel>();

        public IEnumerable<string> Categories { get; set; } = Array.Empty<string>();
    }
}
