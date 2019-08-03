using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IeasteJson2Svg.Models
{
    public class SvgDocument
    {
        public int ID { get; set; }
        [Display(Name = "Name")]
        public string DocumentName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Path")]
        public string DocumentPath { get; set; }
        
    }
}
