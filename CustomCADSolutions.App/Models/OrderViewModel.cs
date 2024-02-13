using CustomCADSolutions.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.App.Models
{
    public class OrderViewModel
    {
        public int CadId { get; set; }

        public string BuyerId { get; set; } = null!;
        
        public string Name { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string OrderDate { get; set; } = null!;
        
        public string BuyerName { get; set; } = null!;
    }
}
