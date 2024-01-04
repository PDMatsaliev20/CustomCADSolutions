using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models
{
    public class ImageInputModel
    {
        [Required]
        public string Description { get; set; } = null!;
    }
}
