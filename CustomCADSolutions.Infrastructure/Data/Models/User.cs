using CustomCADSolutions.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class User
    {
        [Key]
        [Comment("Identification of User")]
        public int Id { get; set; }

        [Required]
        [Comment("Name of User")]
        public string Username { get; set; } = null!;

        [InverseProperty(nameof(Order.Buyer))]
        [MaxLength(DataConstants.User.UsernameMaxLength)]
        [Comment("Orders of User")]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}