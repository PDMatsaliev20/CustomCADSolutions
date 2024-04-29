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

        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [StringLength(CadConstants.NameMaxLength,
            MinimumLength = CadConstants.NameMinLength,
            ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Length))]
        [Display(Name = nameof(DisplayResources.Name), ResourceType = typeof(DisplayResources))]
        public string Name { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [Display(Name = nameof(DisplayResources.Category), ResourceType = typeof(DisplayResources))]
        public int CategoryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [Range(CadConstants.PriceMin, CadConstants.PriceMax,
            ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Range))]
        [Display(Name = nameof(DisplayResources.Price), ResourceType = typeof(DisplayResources))]
        public decimal Price { get; set; }

        public IEnumerable<CategoryModel>? Categories { get; set; }
    }
}
