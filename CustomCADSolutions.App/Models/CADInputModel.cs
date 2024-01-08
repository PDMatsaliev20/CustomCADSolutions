using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models
{
    public class CADInputModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!;
    }
}
