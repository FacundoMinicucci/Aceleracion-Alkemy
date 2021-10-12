using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class ActivitiesDTO
    {
        [Required(ErrorMessage = "The Name field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Content field must be completed.")]
        [MaxLength(65535, ErrorMessage = "You must enter 65535 characters maximum.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

       
        public string Image { get; set; }
    }
}
