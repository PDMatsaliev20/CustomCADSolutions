using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<CAD> CADs { get; set; } = new List<CAD>();
    }
}