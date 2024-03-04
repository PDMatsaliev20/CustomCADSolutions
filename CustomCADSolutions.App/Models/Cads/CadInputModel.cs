using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.App.Resources.Shared;

namespace CustomCADSolutions.App.Models.Cads
{
    public class CadInputModel
    {
        public int Id { get; set; }
        public string? BuyerId { get; set; }

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
        [Display(Name = "File", ResourceType = typeof(SharedResources))]
        public IFormFile CadFile { get; set; } = null!;
        
        [Range(CadConstants.XMin, CadConstants.XMax,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Range))]
        public short X { get; set; }
        
        [Range(CadConstants.YMin, CadConstants.YMax, 
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Range))]
        public short Y { get; set; }

        [Range(CadConstants.ZMin, CadConstants.ZMax, 
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Range))]
        public short Z { get; set; }

        [Range(0, CadConstants.SpinFactorMax * 100,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Range))]
        [Display(Name = "Speed", ResourceType = typeof(SharedResources))]
        public int SpinFactor { get; set; }

        [Display(Name = "Axis", ResourceType = typeof(SharedResources))]
        public char? SpinAxis { get; set; }

        public Category[]? Categories { get; set; }
    }
}
