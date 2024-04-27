using CustomCADSolutions.App.Resources.Shared;
using CustomCADSolutions.Core.Models;
using System.ComponentModel.DataAnnotations;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;

namespace CustomCADSolutions.App.Models.Orders
{
    public class OrderAddModel
    {
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
        [StringLength(OrderConstants.DescriptionMaxLength,
            MinimumLength = OrderConstants.DescriptionMinLength,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Length))]
        [Display(Name = "Description", ResourceType = typeof(SharedResources))]
        public string Description { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Required))]
        [Display(Name = "Category", ResourceType = typeof(SharedResources))]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryModel>? Categories { get; set; }
    }
}
