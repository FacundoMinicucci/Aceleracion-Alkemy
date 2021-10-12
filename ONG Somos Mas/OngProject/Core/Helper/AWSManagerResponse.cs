using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Helper
{
    public class AWSManagerResponse
    {
        public string Message { get; set; }
        public int? Code { get; set; }
        public string Errors { get; set; }
        public string NameImage { get; set; }
        public string URL { get; set; }
    }
}
