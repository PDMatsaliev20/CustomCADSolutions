using static CustomCADSolutions.Infrastructure.Constants.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Core.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(User.UsernameMaxLength, MinimumLength = User.UsernameMinLength, ErrorMessage = LengthErrorMessage)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}