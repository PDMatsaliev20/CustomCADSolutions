using System.ComponentModel.DataAnnotations;

namespace CustomCADs.Domain.Entities
{
    public class Category : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
