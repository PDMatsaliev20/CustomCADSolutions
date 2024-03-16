using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using CustomCADSolutions.App.Resources.Shared;

namespace CustomCADSolutions.App.Models.Orders
{
    public class OrderInputModel
    {
        public int CadId { get; set; }

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
        [StringLength(OrderConstants.DescriptionMaxLength,
            MinimumLength = OrderConstants.DescriptionMinLength,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Length))]
        [Display(Name = "Description", ResourceType = typeof(SharedResources))]
        public string Description { get; set; } = null!;

        public DateTime OrderDate { get; set; }
        
        public OrderStatus Status { get; set; }

        public IEnumerable<Category>? Categories { get; set; }
    }
}
