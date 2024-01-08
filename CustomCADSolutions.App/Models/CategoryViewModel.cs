using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Models
{
    public class CategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<CADViewModel> CADs { get; set; }
    }
}
