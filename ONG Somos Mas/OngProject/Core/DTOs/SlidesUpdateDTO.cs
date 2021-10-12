namespace OngProject.Core.DTOs
{
    using System.ComponentModel.DataAnnotations;


    public class SlidesUpdateDTO
    {
        [Required(ErrorMessage = "The image field must be completed")]
        public string Image { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Text { get; set; }
    }
}
