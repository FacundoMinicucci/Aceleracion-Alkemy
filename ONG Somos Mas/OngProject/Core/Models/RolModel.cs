using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models 
{
    public class RolModel : EntityBase
    {
        [Required(ErrorMessage = "Rol name filed must be completed")]
        [MaxLength(255, ErrorMessage = "The name cannot exceed 255 characters.")]
        public string Name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Description { get; set; }

    }
}
