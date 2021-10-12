using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs
{
    public class TestimonialsRequestDTO
    {
        [Required(ErrorMessage = "The name field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.Text, ErrorMessage = "The name field can only contain text")]
        [RegularExpression(@"^[a-zA-Z]+(\s{0,1}[a-zA-Z])*$", ErrorMessage = "Name field allows only letters")]
        public string Name { get; set; }

        [DataType(DataType.Text, ErrorMessage = "The image field can only contain text")]
        public string Image { get; set; }

        [Required(ErrorMessage = "The content field must be completed.")]
        [MaxLength(65535, ErrorMessage = "You must enter 65535 characters maximum.")]
        [DataType(DataType.MultilineText, ErrorMessage = "The content field can only contain text")]
        public string Content { get; set; }
    }
}
