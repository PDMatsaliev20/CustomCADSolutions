using CustomCADSolutions.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.Models
{
    public class OrderViewModel
    {
        public string Name { get; set; } = null!;

        public Category Category { get; set; }

        public string Description { get; set; } = null!;

        public DateTime OrderDate { get; set; }
    }
}
