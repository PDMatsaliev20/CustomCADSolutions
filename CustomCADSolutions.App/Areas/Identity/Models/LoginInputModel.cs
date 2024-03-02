using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.App.Areas.Identity.Models
{
    public class LoginInputModel
    {
        [TempData]
        public string? ErrorMessage { get; set; }

        [Required]
        [StringLength(62)]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
