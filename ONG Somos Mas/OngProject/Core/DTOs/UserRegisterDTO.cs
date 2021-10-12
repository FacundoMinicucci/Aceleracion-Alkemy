using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "The FirstName field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The LastName field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The email field must be completed.")]
        [MaxLength(320, ErrorMessage = "You must enter 320 characters maximum.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field must be completed.")]
        [MaxLength(20, ErrorMessage = "You must enter 20 characters maximum.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }
    }
}
