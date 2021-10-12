using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models
{
    public class NewsModel : EntityBase
    {
        [Required(ErrorMessage = "The name field must be completed")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The content field must be completed")]
        [MaxLength(65535, ErrorMessage = "You must enter 65535 characters maximum")]
        public string Content { get; set; }

        [Required(ErrorMessage = "The image field must be completed")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum")]
        public string Image { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum")]
        public string Type { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoriesModel Category { get; set; }
    }
}
