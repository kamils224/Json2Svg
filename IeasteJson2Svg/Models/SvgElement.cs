using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IeasteJson2Svg.Models
{
    public class SvgElement
    {
        public int Id { get; set; }
        [Display(Name = "Document ID")]
        public int DocumentId { get; set; }
        [Display(Name = "Attribute name")]
        public string AttributeName { get; set; }
        [Display(Name = "Inner text")]
        public string AttributeInnerText { get; set; }
    }
}
