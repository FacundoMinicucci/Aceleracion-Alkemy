using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models
{
    public class MembersModel : EntityBase
    {
        [Required(ErrorMessage = "The name field must be completed")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
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
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Image { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
