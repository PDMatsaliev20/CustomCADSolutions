using CustomCADSolutions.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models
{
    public class OrderInputModel
    {
        [Required]
        public string CadName { get; set; } = null!;

        [Required]
        public string CadCategory { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
    }
}
