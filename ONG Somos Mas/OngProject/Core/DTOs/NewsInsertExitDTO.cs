using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class NewsInsertExitDTO
    {
        public string Name { get; set; }

        public string Content { get; set; }
       
        public string Image { get; set; }

        public string Type { get; set; }

        public int CategoryId { get; set; }

    }
}
