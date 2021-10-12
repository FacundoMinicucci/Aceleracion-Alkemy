using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs
{
    public class CategoriesOnlyNameDTO
    {
        [Required(ErrorMessage = "The name field must be completed")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum")]
        public string Name { get; set; }
    }
}
