using IeasteJson2Svg.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace IeasteJson2Svg.Tools
{
    public class SvgEditor
    {
        public static MemoryStream GenerateSvgDocument(SvgDocumentEditModel model)
        {
            string tag = model.ElementName;
            string attribute = model.Attribute;

            XmlDocument doc = new XmlDocument();
            doc.Load(model.TemplateDocumentPath);

            var elementList = doc.GetElementsByTagName(tag);

            for (int i = 0; i < elementList.Count; i++)
            {
                string idValue = elementList[i].Attributes[attribute].Value;
                if (model.ElementsForSubstitution.ContainsKey(idValue))
                {
                    elementList[i].InnerText = model.ElementsForSubstitution[idValue][model.ValueIndex];
                }
            }
            var memory = new MemoryStream();

            doc.Save(memory);
            memory.Flush();
            memory.Position = 0;
            return memory;
        }

        public static List<XmlNode> FindEditableElements(string tag, string attribute, string svgFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(svgFilePath);

            var elementList = doc.GetElementsByTagName(tag);
            List<XmlNode> resultList = new List<XmlNode>();
            for (int i = 0; i < elementList.Count; i++)
            {
                string idValue = elementList[i].Attributes[attribute].Value;
                if (!string.IsNullOrEmpty(idValue))
                {
                    resultList.Add(elementList[i]);
                }
            }

            return resultList;
        }
    }
}
