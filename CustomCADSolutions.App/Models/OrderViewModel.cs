using CustomCADSolutions.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models
{
    public class OrderViewModel
    {
        public string? CadName { get; set; }
        public string? CadCategory { get; set; }
        public string? Description { get; set; } 
        public string? OrderDate { get; set; }
    }
}
