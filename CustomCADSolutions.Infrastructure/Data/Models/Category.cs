using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
