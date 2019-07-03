using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IeasteJson2Svg.Models
{
    public class SvgDocumentEditModel
    {
        public string Filename { get; set; }
        public string OutputFilename { get; set; }
        public string ElementName { get; set; }
        public string Attribute { get; set; }
        public Dictionary<string, string> ElementsForSubstitution { get; set; }
    }
}
