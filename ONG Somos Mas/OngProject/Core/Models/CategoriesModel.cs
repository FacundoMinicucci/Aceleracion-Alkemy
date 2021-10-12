using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models
{    
    public class CategoriesModel : EntityBase
    {
        [Required(ErrorMessage = "The name field must be completed")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum")]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum")]       
        public string Description { get; set; }
        
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum")]
        public string Image { get; set; }        
    }
}
