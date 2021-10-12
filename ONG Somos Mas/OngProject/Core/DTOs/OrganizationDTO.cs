using System.Collections.Generic;

namespace OngProject.Core.DTOs
{
    public class OrganizationDTO
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public int? Phone { get; set; }
        public string Address { get; set; }
        public string FacebookURL { get; set; }
        public string LinkedinURL { get; set; }
        public string InstagramURL { get; set; }
        public List<SlideOrganizationDTO> Slides { get; set; }
    }
}
