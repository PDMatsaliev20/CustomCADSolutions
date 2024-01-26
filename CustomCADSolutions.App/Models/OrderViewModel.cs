using CustomCADSolutions.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.App.Models
{
    public class OrderViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Order Name length must be between 1 and 50 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Category is required!")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Order Name length must be between 10 and 5000 characters")]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }
    }
}
