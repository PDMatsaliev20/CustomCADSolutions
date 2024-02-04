using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Models
{
    public class CadInputModel
    {
        [Required(ErrorMessage = "3d model must have a name!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 characters!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "You must choose a category!")]
        [Range(1, 11, ErrorMessage = "Shit")]
        public Category? Category { get; set; } = null!;

        [Required(ErrorMessage = "URL is required!")]
        [RegularExpression(@"^\w{32}$", ErrorMessage = "URL must have exactly 32 symbols, including letters and digits")]
        public string Url { get; set; } = null!;
    }
}
