using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs
{
    public class UserInfoDTO
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
        
        [MaxLength(20, ErrorMessage = "You must enter 20 characters maximum.")]
        public string? Photo { get; set; }

        public int RolId { get; set; }
    }
}
