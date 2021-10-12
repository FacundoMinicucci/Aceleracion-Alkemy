namespace OngProject.Core.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "The email field must be completed.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field must be completed.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
    }
}
