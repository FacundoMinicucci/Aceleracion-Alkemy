namespace OngProject.Core.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class ContactsDTO
    {
        [Required(ErrorMessage = "The name field must be completed")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Name { get; set; }

       
        public string Phone { get; set; }

        [Required(ErrorMessage = "The email field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.EmailAddress)]       
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The message field must be completed.")]
        [MaxLength(320, ErrorMessage = "You must enter 320 characters maximum.")]
        public string Message { get; set; }
    }
}
