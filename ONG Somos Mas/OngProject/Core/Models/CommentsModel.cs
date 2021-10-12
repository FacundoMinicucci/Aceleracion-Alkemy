using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Models
{
    public class CommentsModel : EntityBase
    {
        [Required(ErrorMessage = "The comment field must be completed")]
        [MaxLength(2000, ErrorMessage = "You must enter 2000 characters maximum")]
        public string Body { get; set; }

        public int NewsId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("NewsId")]
        public virtual NewsModel News { get; set; }

        [ForeignKey("UserId")]
        public virtual UsersModel User { get; set; }
    }
}
