using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class CommentsDto
    {
        [Required(ErrorMessage = "The comment field must be completed")]
        [MaxLength(2000, ErrorMessage = "You must enter 2000 characters maximum")]
        public string Body { get; set; }
    }
}
