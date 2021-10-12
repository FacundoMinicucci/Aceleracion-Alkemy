using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class CommentsRequestDTO
    {
        [Required(ErrorMessage = "The news id field must be completed")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int NewsId { get; set; }

        [Required(ErrorMessage = "The user id field must be completed")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The comment field must be completed")]
        [MaxLength(2000, ErrorMessage = "You must enter 2000 characters maximum")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}
