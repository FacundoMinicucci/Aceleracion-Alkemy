using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs
{
    public class MembersRequestDTO
    {
        [Required(ErrorMessage = "The name field must be completed")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.Text, ErrorMessage = "The name field can only contain text")]
        [RegularExpression(@"^[a-zA-Z]+(\s{0,1}[a-zA-Z])*$", ErrorMessage = "Name field allows only letters")]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.Url)]
        public string FacebookURL { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.Url)]
        public string InstagramURL { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.Url)]
        public string LinkedinURL { get; set; }

        [Required(ErrorMessage = "The image field must be completed")]
        [DataType(DataType.Text, ErrorMessage = "The image field can only contain text")]
        public string Image { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
