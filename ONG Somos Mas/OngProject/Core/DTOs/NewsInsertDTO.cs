using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class NewsInsertDTO
    {

        [Required(ErrorMessage = "The name field must be completed")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The content field must be completed")]
        [MaxLength(65535, ErrorMessage = "You must enter 65535 characters maximum")]
        public string Content { get; set; }

        [Required(ErrorMessage = "The image field must be completed")]
        public string Image { get; set; }

        public int CategoryId { get; set; }

    }
}
