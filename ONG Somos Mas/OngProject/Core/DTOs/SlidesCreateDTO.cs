using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class SlidesCreateDTO
    {
        [Required(ErrorMessage = "The image field must be completed")]
        public string Image { get; set; }

        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Text { get; set; }

        public int Order { get; set; }
    }
}
