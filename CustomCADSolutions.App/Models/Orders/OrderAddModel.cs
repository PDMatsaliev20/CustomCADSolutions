using CustomCADSolutions.App.Resources.Shared;
using CustomCADSolutions.Core.Models;
using System.ComponentModel.DataAnnotations;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;

namespace CustomCADSolutions.App.Models.Orders
{
    public class OrderAddModel
    {
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
        [StringLength(OrderConstants.DescriptionMaxLength,
            MinimumLength = OrderConstants.DescriptionMinLength,
            ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Length))]
        [Display(Name = nameof(DisplayResources.Description), ResourceType = typeof(DisplayResources))]
        public string Description { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [Display(Name = nameof(DisplayResources.Category), ResourceType = typeof(DisplayResources))]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryModel>? Categories { get; set; }
    }
}
