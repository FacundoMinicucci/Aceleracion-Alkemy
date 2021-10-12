using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class TestimonialUpdateDTO
    {
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Name { get; set; }

        public string Image { get; set; }

        [MaxLength(65535, ErrorMessage = "You must enter 65535 characters maximum.")]
        public string Content { get; set; }
    }
}
