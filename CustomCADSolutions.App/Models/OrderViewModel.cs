using CustomCADSolutions.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string? Description { get; set; } 
    }
}
