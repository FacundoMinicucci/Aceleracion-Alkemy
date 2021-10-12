using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class UsersUpdateDTO
    {
        [Required(ErrorMessage = "The FirstName field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The LastName field must be completed.")]
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string LastName { get; set; }

        public string Photo { get; set; }
    }
}
