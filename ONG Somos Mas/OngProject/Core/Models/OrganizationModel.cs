using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models
{
    public class OrganizationModel : EntityBase
    {
        [Required(ErrorMessage = "The name field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The image field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Image { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Address { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public int? Phone { get; set; }

        [Required(ErrorMessage = "The email field must be completed.")]
        [MaxLength(320, ErrorMessage = "You must enter 320 characters maximum.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The welcome text field must be completed.")]
        [MaxLength(500, ErrorMessage = "You must enter 500 characters maximum.")]
        public string WelcomeText { get; set; }

        [MaxLength(2000, ErrorMessage = "You must enter 2000 characters maximum.")]
        public string AboutUsText { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.Url)]
        public string FacebookURL { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.Url)]
        public string LinkedinURL { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        [DataType(DataType.Url)]
        public string InstagramURL { get; set; }
    }
}