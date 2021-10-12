using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class SlidesDto
    {
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Image { get; set; }
        public int Order { get; set; }
    }
}
