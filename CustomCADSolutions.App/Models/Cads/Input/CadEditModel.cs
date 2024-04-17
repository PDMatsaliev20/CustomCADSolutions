using CustomCADSolutions.App.Resources.Shared;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.App.Models.Cads.Input
{
    public class CadEditModel
    {
        public int Id { get; set; }
        
        public bool IsValidated { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Required))]
        [StringLength(CadConstants.NameMaxLength,
            MinimumLength = CadConstants.NameMinLength,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Length))]
        [Display(Name = "Name", ResourceType = typeof(SharedResources))]
        public string Name { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Required))]
        [Display(Name = "Category", ResourceType = typeof(SharedResources))]
        public int CategoryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Required))]
        [Range(CadConstants.PriceMin, CadConstants.PriceMax,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Range))]
        [Display(Name = "Price", ResourceType = typeof(SharedResources))]
        public decimal Price { get; set; }

        [Range(CadConstants.XMin, CadConstants.XMax,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Range))]
        public int X { get; set; }

        [Range(CadConstants.YMin, CadConstants.YMax,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Range))]
        public int Y { get; set; }

        [Range(CadConstants.ZMin, CadConstants.ZMax,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Range))]
        public int Z { get; set; }

        [RegularExpression(CadConstants.SpinAxisRegEx,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.InvalidAxis))]
        [Display(Name = "Axis", ResourceType = typeof(SharedResources))]
        public char? SpinAxis { get; set; }

        public IEnumerable<CategoryModel>? Categories { get; set; }
    }
}
