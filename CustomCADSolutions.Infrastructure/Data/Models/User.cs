using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [InverseProperty(nameof(Order.Buyer))]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}