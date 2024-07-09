using CustomCADs.App.Resources.Shared;
using static CustomCADs.Infrastructure.Data.DataConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADs.Core.Models;

namespace CustomCADs.App.Models.Orders
{
    public class OrderEditModel
    {
        public int Id { get; set; }

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
