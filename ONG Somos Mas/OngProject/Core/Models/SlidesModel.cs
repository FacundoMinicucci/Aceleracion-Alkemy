using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models
{
    public class SlidesModel : EntityBase
    {
        [MaxLength(255, ErrorMessage = "You must enter 255 characters maximum.")]
        public string Image { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }
       
        public int OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual OrganizationModel Organization { get; set; }
    }
}
