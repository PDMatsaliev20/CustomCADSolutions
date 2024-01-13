using CustomCADSolutions.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Core.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}