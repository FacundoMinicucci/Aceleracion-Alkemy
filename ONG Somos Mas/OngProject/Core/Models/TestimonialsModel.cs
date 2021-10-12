using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models
{
    public class TestimonialsModel : EntityBase
    {
        [Required(ErrorMessage = "The name field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Image { get; set; }

        [Required(ErrorMessage = "The content field must be completed.")]
        [MaxLength(65535, ErrorMessage = "You must enter 65535 characters maximum.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}
