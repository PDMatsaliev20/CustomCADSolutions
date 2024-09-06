using System.ComponentModel.DataAnnotations;

namespace CustomCADs.Infrastructure.Data.Entities
{
    public class PCategory 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
