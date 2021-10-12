namespace OngProject.Core.Models
{
    using System.ComponentModel.DataAnnotations;   

    public class ContactsModel: EntityBase
    {
        [Required(ErrorMessage = "The name field must be completed")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Name { get; set; }

        [Range(10, 16,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The email field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The message field must be completed.")]
        [MaxLength(320, ErrorMessage = "You must enter 320 characters maximum.")]
        public string Message { get; set; }
    }
}
