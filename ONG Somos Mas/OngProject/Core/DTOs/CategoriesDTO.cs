namespace OngProject.Core.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class CategoriesDTO
    {
        [Required(ErrorMessage = "The name field must be completed")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum")]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum")]
        public string Description { get; set; }

        public string Image { get; set; }
    }
}
