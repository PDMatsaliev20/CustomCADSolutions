using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Core.Models
{
    public class CadModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Order Name length must be between 1 and 50 characters!")]
        public string Name { get; set; } = null!;

        public DateTime? CreationDate { get; set; }

        [Required]  
        public string Url { get; set; } = null!;

        [Required(ErrorMessage = "Category is required!")]
        public Category Category { get; set; }

        [Required]
        public ICollection<OrderModel>? Orders { get; set; }
    }
}
